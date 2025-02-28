import { push } from "svelte-spa-router";
import { UserContext } from "./UserContext";

export function ensureLoggedIn() {
    if (!new UserContext().isAuth()) {
        push("/account/login");
    }
}