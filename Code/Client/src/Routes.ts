import HomePage from "./Pages/Home/HomePage.svelte";
import ContactPage from "./Pages/Contact/ContactPage.svelte";
import NotFound from "./Pages/NotFound.svelte";
import Routes from "./Pages/Account/Routes";

export default {
    ...Routes,
    "/": HomePage,
    "/contact": ContactPage,
    "*": NotFound
}