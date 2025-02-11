import HomePage from "./Pages/Home/HomePage.svelte";
import ContactPage from "./Pages/Contact/ContactPage.svelte";
import NotFound from "./Pages/NotFound.svelte";

export default {
    "/": HomePage,
    "/contact": ContactPage,
    "*": NotFound
}