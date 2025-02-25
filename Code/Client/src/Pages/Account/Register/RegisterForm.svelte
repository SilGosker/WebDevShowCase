<script lang="ts">
    import Swal from "sweetalert2";
    import FormField from "../../../Components/Form/FormField.svelte";
    import { ApiContext } from "../../../Functions/ApiContext";

    const email = {
        valid: false,
        value: "",
    };

    const repeatPassword = {
        valid: false,
        value: "",
    };

    const password = {
        valid: false,
        value: "",
    };

    let repeatPasswordErrMessage = '';

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
        if ([repeatPassword, password, email].some(x => !x.valid)) {
            return;
        }
        if (repeatPassword.value != password.value) {
            repeatPasswordErrMessage = 'Wachtwoorden komen niet overeen';
            return;
        } else {
            repeatPasswordErrMessage = '';
        }

        var context = new ApiContext();
        const result = await context.submitRegistrationForm({
            email: email.value,
            password: password.value
        });

        await Swal.fire({
            title: result
            ? "Account aangemaakt!"
            : "Er is iets fout gegaan.",
            text: result
                ? "Je account is aangemaakt. Na het wegklikken verwijzen we je naar de inlogpagina."
                : "Je account kon niet aangemaakt worden, waarschijnlijk omdat je e-mail al bestaat.",
            icon: result
            ? "success"
            : "error",
        });

        window.location.href = '/#/account/login';
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
            type="password"
            maxLength={255}
        />
    </div>
    <div class=" md:w-1/2 w-3/4">
        <FormField
            onchange={(value, valid) => updateValidity(repeatPassword, value, valid)}
            name="Wachtwoordherhaling"
            errorMessage={repeatPasswordErrMessage}
            type="password"
            maxLength={255}
        />
    </div>

    <button type="submit" class="py-2 px-4 border-sky-600 bg-sky-400 text-white rounded-lg">
        Versturen
    </button>
</form>
