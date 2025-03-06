<script>
    import { ensureLoggedIn } from "../../../Functions/EnsureLoggedIn";
    import CreatePlantForm from "./CreatePlantForm.svelte";
    ensureLoggedIn();
</script>

<div class="flex flex-col justify-center w-full">
    <h1 class="text-center text-2xl w-full text-gray-900">Plant aanmaken</h1>
    <CreatePlantForm />
</div>

