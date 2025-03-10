import { push } from "svelte-spa-router";
import { UserContext } from "./UserContext";
import Swal from "sweetalert2";
export class ApiContext {
    async updatePlant(form: { duration: number; name: string; id: number, regeneratePassword: boolean })
    : Promise<{id: number, password: string | null} | null> {
        console.log(form);
        const response = await this.sendRequest("plants/update/" + form.id, "PUT", form);
        if (!response.ok) {
            return null;
        }

        return await response.json();
    }

    private baseUrl: string = "https://localhost:7196/";
    private userContext = new UserContext();

    private async sendRequest(url: string, method: string, body: object = null): Promise<Response> {
        const headerObj = {
            'Content-Type': 'application/json',
        };

        if (this.userContext.isAuth()) {
            const bearerToken = this.userContext.token();
            headerObj['Authorization'] = 'Bearer ' + bearerToken;
        }

        const args = {
            method: method,
            headers: headerObj,
            body: undefined,
        }

        if (body) {
            args.body = JSON.stringify(body)
        }
        try {
            const response = await fetch(this.baseUrl + url, args);
            if (response.status === 401) {
                push("/account/login");
            } else if (response.status === 500) {
                throw new Error("internal server error");
            }
            return response;
        } catch (e) {
            Swal.fire({
                title: "Er is iets fout gegaan",
                text: `Een onverwachte fout is opgetreden. Probeer het later opnieuw. Als de fout blijft optreden, neem contact op met de ontwikkelaar.`
            });
        }

    }

    async submitContactForm(formObj: {
        name: string,
        email: string,
        phone: string,
        message: string,
        subject: string
    }): Promise<boolean> {
        try {
            return (await this.sendRequest("mailing/send", "POST", formObj)).ok;
        } catch (e) {
        }
        return false;
    }

    async submitRegistrationForm(formObj: { email: string, password: string }) {
        try {
            return (await this.sendRequest("account/register",
                "POST",
                formObj)).ok;
        } catch (e) {
        }
        return false;
    }

    async submitLogin(formObj: { email: string; password: string; }) {
        try {
            var response = await this.sendRequest("account/login",
                "POST",
                formObj);
            if (!response.ok) {
                return false;
            }
            const result: { token: string, role: string } = await response.json();
            return result;
        } catch (e) {
        }
        return false;
    }

    async getPlants(): Promise<{ name: string, id: number }[]> {
        const response = await this.sendRequest("plants", "GET");
        if (!response.ok) {
            return [];
        }
        return await response.json();
    }

    async getPlant(id: number): Promise<{ name: string, id: number, data: [] }> {
        const response = await this.sendRequest("plants/" + id, "GET");
        if (!response.ok) {
            return null;
        }

        return await response.json();
    }

    async plantIsConnected(id: number): Promise<Boolean> {
        const response = await this.sendRequest("plants/state/" + id, "GET");
        const obj = await response.json();
        return obj.state === "Connected";
    }

    async createPlant(formObj: { duration: number; name: string; }): Promise<"ReachedLimit" | "SomethingWentWrong" | { id: number; password: string; }> {
        const response = await this.sendRequest("plants/create", "POST", formObj);
        if (!response || !response.ok) {
            const errors = (await response.json() as { errors: { [key: string]: string } }).errors;
            if (Object.keys(errors).length) {
                return "SomethingWentWrong";
            }
            return "ReachedLimit";
        }

        const obj = await response.json();
        return obj;
    }

}