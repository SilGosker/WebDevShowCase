export class ApiContext {
    private baseUrl : string = "https://localhost:7196/";
    private async sendRequest(url : string, method : string, body : object) : Promise<Response> {
        return await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body)
        });
    }
    async submitContactForm(formObj : { name, email, phone, message, subject }) : Promise<boolean> {
        try {
            return (await this.sendRequest(this.baseUrl + "mailing/send", "POST", formObj)).ok;
        } catch (e) {
        }
        return false;
    }
}