
<script lang="ts">
    import Swal from "sweetalert2";
    import FormField from "../../../Components/Form/FormField.svelte";
    import { ApiContext } from "../../../Functions/ApiContext";
    import { push } from "svelte-spa-router";
    import { ensureLoggedIn } from "../../../Functions/EnsureLoggedIn";
    
    ensureLoggedIn();

    const name = {
        valid: false,
        value: "",
    };

    const duration = {
        valid: false,
        value: 0,
    };

    function updateValidity(
        object: { valid: boolean; value: string | number },
        value: string | number,
        valid: boolean,
    ) {
        object.value = value;
        object.valid = valid;
    }

    async function submit(event : Event) {
        event.preventDefault();
        if ([duration, name].some(x => !x.valid)) {
            return;
        }

        var context = new ApiContext();
        const result = await context.createPlant({
            duration: duration.value,
            name: name.value,
        });
        
        if (result === "SomethingWentWrong") {
            Swal.fire({
                title: "Plant kon niet aangemaakt worden",
                text: "De plant kon niet aangemaakt worden. Probeer het later nog een keer.",
                icon: "error"
            });
            return;
        } else if (result === "ReachedLimit") {
            Swal.fire({
                title: "Plant kon niet aangemaakt worden",
                text: "U heeft uw limiet van 5 planten bereikt. Verwijder een plant of maak een nieuw account aan.",
                icon: "error"
            });
            return;
        }

        await Swal.fire({
                title: "Plant aangemaakt!",
                text: "De plant is aangemaakt. Om de hydrocomputer te verbinden" +
                       "met de server is een token nodig. Het token is: '" + result.password + 
                       "'. Sla deze goed op, deze kan niet teruggehaald worden.",
                icon: "success"
            });
        
        push("/plants/" + result.id);
    }
</script>

<form
    action=""
    method="get"
    class="p-2 w-full flex-col h-full flex items-center justify-center flex-wrap"
    on:submit={submit}>
    <span class="text-gray-500 ml-2">* Verplichte velden</span>
    <div class="md:w-1/2 w-3/4">
        <FormField
            onchange={(value, valid) => updateValidity(name, value, valid)}
            name="name"
            type="text" 
            maxLength={255}
        />
    </div>

    <div class="md:w-1/2 w-3/4">
        <FormField
            onchange={(value, valid) => updateValidity(duration, value, valid)}
            name="duratie (seconden)"
            type="number"
            max={300}
            min={0}
        />
    </div>

    <button type="submit" class="py-2 px-4 border-sky-600 bg-sky-400 text-white rounded-lg">
        Versturen
    </button>
</form>
