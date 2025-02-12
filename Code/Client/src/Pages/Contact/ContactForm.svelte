<script lang="ts">
    import FormField from "../../Components/Form/FormField.svelte";
    import { ApiContext } from "../../Functions/ApiContext.js";
    import Swal from "sweetalert2";
    import { Recaptcha } from "svelte-recaptcha-v2";
    import {
        message,
        subject,
        phone,
        email,
        lastName,
        firstName,
        updateValidity,
    } from "./ContactData";

    let isLoading = false;
    let showSubmitBtn = false;

    function captchaCleared() {
        showSubmitBtn = true;
    }

    async function submit(event: Event) {
        event.preventDefault();
        const validationArr = [
            message,
            subject,
            phone,
            email,
            lastName,
            firstName,
        ];
        if (validationArr.some((e) => !e.valid)) {
            return;
        }

        isLoading = true;

        var context = new ApiContext();
        var result = await context.submitContactForm({
            message: message.value,
            subject: subject.value,
            phone: phone.value,
            email: email.value,
            name: (firstName.value + " " + lastName.value).trim(),
        });

        await Swal.fire({
            title: result ? "Verstuurd!" : "Er is iets fout gegaan",
            text: result
                ? "Je bericht is aangekomen. Binnen een paar dagen zullen we contact met je opnemen"
                : "We kunnen je bericht op het moment niet versturen. Probeer het later opnieuw.",
            icon: result ? "success" : "error",
        });

        isLoading = false;
    }
</script>

<form action="" on:submit={submit} method="get" class="p-2">
    <span class="text-gray-500 ml-2">* Verplichte velden</span>
    <div class="w-full h-full flex flex-wrap">
        <div class="w-1/2">
            <FormField
                onchange={(value, valid) =>
                    updateValidity(firstName, value, valid)}
                name="voornaam"
            />
        </div>

        <div class="w-1/2">
            <FormField
                onchange={(value, valid) =>
                    updateValidity(lastName, value, valid)}
                name="achternaam"
            />
        </div>

        <div class="w-full">
            <FormField
                onchange={(value, valid) => updateValidity(email, value, valid)}
                type="email"
                name="Uw email"
            />
        </div>

        <div class="w-full">
            <FormField
                onchange={(value, valid) => updateValidity(phone, value, valid)}
                type="tel"
                name="Uw telefoonnummer"
                required={false}
            />
        </div>

        <div class="w-full">
            <FormField
                onchange={(value, valid) =>
                    updateValidity(message, value, valid)}
                maxLength={200}
                name="Onderwerp"
            />
        </div>

        <div class="w-full">
            <FormField
                type="longtext"
                onchange={(value, valid) =>
                    updateValidity(subject, value, valid)}
                name="Bericht"
                required={true}
            />
        </div>

        <div class="w-full flex justify-center">
            {#if isLoading}
                <button
                    class="mt-3 py-2 px-4 border-sky-600 bg-sky-400 text-white rounded-lg"
                >
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 100 100"
                        preserveAspectRatio="xMidYMid"
                        width="50"
                        height="50"
                        style="shape-rendering: auto; display: block;"
                    >
                        <g>
                            <circle
                                stroke-dasharray="164.93361431346415 56.97787143782138"
                                r="35"
                                stroke-width="10"
                                stroke="white"
                                fill="none"
                                cy="50"
                                cx="50"
                            >
                                <animateTransform
                                    keyTimes="0;1"
                                    values="0 50 50;360 50 50"
                                    dur="1s"
                                    repeatCount="indefinite"
                                    type="rotate"
                                    attributeName="transform"
                                ></animateTransform>
                            </circle>
                            <g></g>
                        </g>
                    </svg>
                </button>
            {:else if showSubmitBtn}
                <button type="submit" class="py-2 px-4 border-sky-600 bg-sky-400 text-white rounded-lg">
                    Versturen
                </button>
            {:else}
                <Recaptcha
                sitekey={"6LezsNMqAAAAACJzRSKBytTCjIU5ClHsTirQenV7"}
                size={"normal"}
                on:success={captchaCleared}
                />

            {/if}
        </div>
    </div>
</form>
