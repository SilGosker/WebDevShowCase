import PlantDetailsPage from "./Details/PlantDetailsPage.svelte";
import PlantIndexPage from "./Index/PlantIndexPage.svelte";

export default {
    "/plants": PlantIndexPage,
    "/plants/:id": PlantDetailsPage,
};