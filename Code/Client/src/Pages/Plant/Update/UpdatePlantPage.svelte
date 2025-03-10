<script>
    import Loader from "../../../Components/Loader.svelte";
    import { ApiContext } from "../../../Functions/ApiContext";
    import { ensureLoggedIn } from "../../../Functions/EnsureLoggedIn";
    import UpdatePlantForm from "./UpdatePlantForm.svelte";
    import { onMount } from "svelte";
    ensureLoggedIn();

    let plant = null;

    const apiContext = new ApiContext();
    onMount(async () => {
        const id = parseInt(
            window.location.href.substring(
                window.location.href.lastIndexOf("/") + 1,
            ),
        );
        plant = await apiContext.getPlant(id);
    });
</script>

<div class="flex flex-col justify-center w-full">
    <h1 class="text-center text-2xl w-full text-gray-900">Plant updaten</h1>

    {#if !plant}
        <div class="flex justify-center">
            <Loader />
        </div>
    {:else}
        <UpdatePlantForm {plant} />
    {/if}
</div>
