export class UserContext {
    private _token: string | undefined;
    private _name : string;
    private _role: string;

    static #handlers = [];
    static addEventListener(event: "authChanged", handler: (loggedIn: boolean) => void) {
        UserContext.#handlers.push(handler);
    }

    private static authChanged(loggedIn: boolean) {
        UserContext.#handlers.forEach(x => x(loggedIn));
    }

    constructor() {
        this._token = localStorage.getItem("token");
        const credentials = JSON.parse(localStorage.getItem("credentials"));
        if (credentials) {
            this._name = credentials.name;
            this._role = credentials.role;
            UserContext.authChanged(true);
        }
    }

    setAuth(credentials : { name: string, role: string, token : string}) {
        this._token = credentials.token;
        this._name = credentials.name;
        this._role = credentials.role;
        localStorage.setItem("token", this._token); 
        localStorage.setItem("credentials", JSON.stringify({
            name: credentials.name,
            role: credentials.role
        }));
        UserContext.authChanged(true);
    }
    
    clear() {
        localStorage.removeItem("token");
        localStorage.removeItem("credentials");
        this._token = null;
        this._name = '';
        this._role = '';
        UserContext.authChanged(false);
     }
    
    isAuth() {
        return !!(this._name && this._token && this._role);
    }

    name() {
        return this._name;
    }

    token() {
        return this._token;
    }

    role() {
        return this._role;
    }

    isAdmin() {
        return this._role === "Admin";
    }
}