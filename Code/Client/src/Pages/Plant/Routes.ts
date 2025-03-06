import CreatePlantPage from "./Create/CreatePlantPage.svelte";
import PlantDetailsPage from "./Details/PlantDetailsPage.svelte";
import PlantIndexPage from "./Index/PlantIndexPage.svelte";

export default {
    "/plants": PlantIndexPage,
    "/plants/create": CreatePlantPage,
};

export const maps = [
    {
        key: /\/plants\/[0-9]{1,}/, 
        value: PlantDetailsPage
    }
];