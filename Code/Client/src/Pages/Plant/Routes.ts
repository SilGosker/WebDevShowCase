import CreatePlantPage from "./Create/CreatePlantPage.svelte";
import PlantDetailsPage from "./Details/PlantDetailsPage.svelte";
import PlantIndexPage from "./Index/PlantIndexPage.svelte";
import UpdatePlantPage from "./Update/UpdatePlantPage.svelte";

export default {
    "/plants": PlantIndexPage,
    "/plants/create": CreatePlantPage,
};

export const maps = [
    {
        key: /\/plants\/[0-9]{1,}/, 
        value: PlantDetailsPage
    },
    {
        key: /\/plants\/update\/[0-9]{1,}/, 
        value: UpdatePlantPage
    }
];