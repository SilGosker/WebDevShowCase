<script lang="ts">
    import { onMount } from "svelte";
    import { ApiContext } from "../../../Functions/ApiContext";
    import { ensureLoggedIn } from "../../../Functions/EnsureLoggedIn";
    import Card from "../../../Components/Card.svelte";
    import Loader from "../../../Components/Loader.svelte";
    ensureLoggedIn();
    const apiContext = new ApiContext();
    let plants: { state: Boolean | undefined; name: string; id: number }[] = [];
    let loading = true;

    onMount(async () => {
        plants = (await apiContext.getPlants()).map((x) => ({
            ...x,
            state: undefined,
        }));

        plants.forEach(async (plant) => {
            await new Promise((r) => setTimeout(r, Math.random() * 1000 + 50));
            plant.state = await apiContext.plantIsConnected(plant.id);
            // trigger rerender
            plants = plants;
        });
        loading = false;
    });
</script>

<div class="flex flex-col justify-center w-full">
    <h1 class="text-center text-2xl w-full text-gray-900">Uw planten</h1>
    <div class="w-full flex justify-center flex-wrap">
        {#if loading}
            <i>Laden...</i>
        {:else}
            {#if !plants.length}
                <i>U heeft nog geen planten</i>
            {/if}

            {#each plants as plant}
                <Card link="plants/{plant.id}" title={plant.name}>
                    {#if plant.state == undefined}
                        <div class="flex justify-center">
                            <Loader />
                        </div>
                    {:else}
                        <div class="my-3 flex justify-between">
                            <span>Status:</span>
                            <span
                                class={plant.state
                                    ? "text-green-500"
                                    : "text-red-500"}
                            >
                                {plant.state ? "verbonden" : "Niet verbonden"}
                            </span>
                        </div>
                    {/if}
                </Card>
            {/each}
        {/if}
    </div>
</div>
