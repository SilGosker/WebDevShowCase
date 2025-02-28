import NotFound from "./Pages/NotFound.svelte";
import AccountRoutes from "./Pages/Account/Routes";
import PlantRoutes from "./Pages/Plant/Routes";
import HomeRoutes from "./Pages/Default/Routes";
export default {
    ...AccountRoutes,
    ...PlantRoutes,
    ...HomeRoutes,
    "*": NotFound
}