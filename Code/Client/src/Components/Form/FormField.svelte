<script lang="ts">
    let internalErrorMessage = "";
    export let value: string | number = undefined;
    export let onchange: (value: string | number, valid: boolean) => void =
        undefined;
    export let name: string;
    export let maxLength: number = undefined;
    export let minLength: number = undefined;
    export let required: boolean = true;
    export let type:
        | "text"
        | "string"
        | "email"
        | "number"
        | "tel"
        | "longtext"
        | "password"
        | "password-nocheck" = "text";
    export let min: number = undefined;
    export let max: number = undefined;
    export let errorMessage: string = "";

    function valueChanged(event: Event) {
        internalErrorMessage = "";
        const value = (event.target as HTMLInputElement).value as string;

        if (!value && required) {
            internalErrorMessage = name + " is verplicht";
            onchange?.(value, false);
            return;
        }

        if (maxLength && value.length > maxLength) {
            internalErrorMessage =
                name + " mag niet langer zijn dan " + maxLength + " karakters.";
            onchange?.(value, false);
            return;
        }

        if (minLength && value.length < minLength) {
            internalErrorMessage =
                name + " mag niet korter zijn dan " + minLength + " karakters.";
            onchange?.(value, false);
            return;
        }

        if (type == "password") {
            const passwordRegex =
                /^.*(?=.{8,})(?=.*[a-zA-Z])(?=.*\d)(?=.*[!#$%&? "]).*$/;
            if (!value.match(passwordRegex)) {
                internalErrorMessage =
                    name +
                    " moet minimaal 8 karakters lang zijn, een getal, hoofdletter en een speciaal karakter bevatten.";
                onchange?.(value, false);
                return;
            }
        }

        if (type == "email") {
            const emailRegex =
                /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
            if (!value.match(emailRegex)) {
                internalErrorMessage = name + " is een ongeldige e-mail adres";
                onchange?.(value, false);
                return;
            }
        }

        if (type == "number") {
            try {
                const val = parseInt(value);
                if (val > max) {
                    internalErrorMessage =
                        name + " mag niet groter zijn dan " + max;
                    onchange?.(value, false);
                    return;
                }
                if (val < min) {
                    internalErrorMessage =
                        name + " mag niet kleiner zijn dan " + min;
                    onchange?.(value, false);
                    return;
                }
            } catch (e) {
                internalErrorMessage = name + " is een ongeldig getal";
                onchange?.(value, false);
                return;
            }
        }
        onchange?.(value, true);
    }
</script>

<div class="p-2 w-full">
    {#if type === "longtext"}
        <textarea
            class="h-64 w-full border p-2 rounded {errorMessage ||
            internalErrorMessage
                ? 'border-red-600'
                : ''}"
            {required}
            on:change={valueChanged}
            maxlength={maxLength}
            minlength={minLength}
            placeholder={name + (required ? "*" : "")}>{value || ""}</textarea
        >
    {:else}
        <input
            type={type === "password-nocheck" ? "password" : type}
            class="w-full border rounded p-2 {errorMessage ||
            internalErrorMessage
                ? 'border-red-600'
                : ''}"
            {required}
            on:change={valueChanged}
            maxlength={maxLength}
            minlength={minLength}
            {min}
            {max}
            autocomplete={type == "password" ? "off" : "on"}
            placeholder={name + (required ? "*" : "")}
            value={value || ""}
        />
    {/if}
    <small class="text-red-600">{errorMessage || internalErrorMessage}</small>
</div>
