<script lang="ts">
    import { onMount } from "svelte";
    import { ApiContext } from "../../../Functions/ApiContext";
    import Loader from "../../../Components/Loader.svelte";

    const apiContext = new ApiContext();
    let plant = null;

    onMount(async () => {
        const id = parseInt(window.location.href.substring(window.location.href.lastIndexOf('/') + 1));
        plant = await apiContext.getPlant(id);
    });

</script>

<div class="flex justify-center w-full">
    {#if plant}
    <h1 class="text-center text-2xl w-full text-gray-900">{plant.name}</h1>
    {:else}
        <Loader width={200} />
    {/if}
</div>
