
<script lang="ts">
    import Swal from "sweetalert2";
    import FormField from "../../../Components/Form/FormField.svelte";
    import { ApiContext } from "../../../Functions/ApiContext";
    import { UserContext } from "../../../Functions/UserContext";
    import { push } from "svelte-spa-router";

    var userContext = new UserContext();
    if (userContext.isAuth()) {
        push("/plants");
    }

    const email = {
        valid: false,
        value: "",
    };

    const password = {
        valid: false,
        value: "",
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
        if ([password, email].some(x => !x.valid)) {
            return;
        }

        var context = new ApiContext();
        const result = await context.submitLogin({
            email: email.value,
            password: password.value
        });
        
        if (!result) {
            Swal.fire({
                title: "Kon niet inloggen",
                text: "Email/wachtwoord combinatie was niet bekend.",
                icon: "error"
            });
            return;
        }

        userContext.setAuth({
            token: result.token,
            role: result.role,
            name: email.value,
        });
        
        push("/plants");
    }
</script>

<form
    action=""
    class="p-2 w-full flex-col h-full flex items-center justify-center flex-wrap"
    on:submit={submit}
    method="get"
>
    <span class="text-gray-500 ml-2">* Verplichte velden</span>
    <div class="md:w-1/2 w-3/4">
        <FormField
            onchange={(value, valid) => updateValidity(email, value, valid)}
            name="email"
            type="email"
            maxLength={255}
        />
    </div>

    <div class="md:w-1/2 w-3/4">
        <FormField
            onchange={(value, valid) => updateValidity(password, value, valid)}
            name="Wachtwoord"
            type="password-nocheck"
            maxLength={255}
        />
    </div>

    <button type="submit" class="py-2 px-4 border-sky-600 bg-sky-400 text-white rounded-lg">
        Versturen
    </button>
</form>
