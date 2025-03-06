import NotFound from "./Pages/NotFound.svelte";
import AccountRoutes from "./Pages/Account/Routes";
import PlantRoutes, { maps } from "./Pages/Plant/Routes";
import HomeRoutes from "./Pages/Default/Routes";

const map = new Map();

const routes = {
    ...AccountRoutes,
    ...PlantRoutes,
    ...HomeRoutes,
    "*": NotFound
}

for (const key in maps) {
    const route = maps[key];
    map.set(route.key, route.value);
}

for(const key in routes) {
    map.set(key, routes[key]);
}

export default map;