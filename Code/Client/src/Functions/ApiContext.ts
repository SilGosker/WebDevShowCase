import { UserContext } from "./UserContext";
export class ApiContext {
    private baseUrl: string = "https://localhost:7196/";
    private userContext = new UserContext();

    private async sendRequest(url: string, method: string, body: object): Promise<Response> {
        const headerObj = {
            'Content-Type': 'application/json',
        };
        
        if (this.userContext.isAuth()) {
            const bearerToken = this.userContext.token();
            headerObj['Authorization'] = 'Bearer ' + bearerToken;
        }

        return await fetch(url, {
            method: method,
            headers: headerObj,
            body: JSON.stringify(body)
        });
    }

    async submitContactForm(formObj: {
        name: string,
        email: string,
        phone: string,
        message: string,
        subject: string
    }): Promise<boolean> {
        try {
            return (await this.sendRequest(this.baseUrl + "mailing/send", "POST", formObj)).ok;
        } catch (e) {
        }
        return false;
    }

    async submitRegistrationForm(formObj: { email: string, password: string }) {
        try {
            return (await this.sendRequest(this.baseUrl + "account/register",
                "POST",
                formObj)).ok;
        } catch (e) {
        }
        return false;
    }

    async submitLogin(formObj: { email: string; password: string; }) {
        try {
            var response = await this.sendRequest(this.baseUrl + "account/register",
                "POST",
                formObj);
            if (!response.ok) {
                return false;
            }
            const result : { token: string, role : string } = await response.json();
            return result;
        } catch (e) {
        }
        return false;
    }
}