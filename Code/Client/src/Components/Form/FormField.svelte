<script lang="ts">
    let errorMsg = "";
    export let value: string | number = undefined;
    export let onchange: (value: string | number, valid: boolean) => void =
        undefined;
    export let name: string;
    export let maxLength: number = undefined;
    export let required: boolean = true;
    export let type:
        | "text"
        | "string"
        | "email"
        | "number"
        | "tel"
        | "longtext" = "text";

    function valueChanged(event: Event) {
        errorMsg = "";
        const value = (event.target as HTMLInputElement).value as string;

        if (!value && required) {
            errorMsg = name + " is verplicht";
            onchange?.(value, false);
            return;
        }

        if (maxLength && value.length > maxLength) {
            errorMsg =
                name + " mag niet langer zijn dan " + maxLength + " karakters.";
            onchange?.(value, false);
            return;
        }

        if (type == "email") {
            const emailRegex =
                /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
            if (!value.match(emailRegex)) {
                errorMsg = name + " is een ongeldige e-mail adres";
                onchange?.(value, false);
                return;
            }
        }

        if (type == "number") {
            errorMsg = name + " is een ongeldig getal";
            try {
                parseInt(value);
            } catch (e) {
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
            class="h-64 w-full border p-2 rounded {errorMsg
                ? 'border-red-600'
                : ''}"
            {required}
            on:change={valueChanged}
            maxlength={maxLength || undefined}
            placeholder={name + (required ? "*" : "")}
            value={value || ""}
        ></textarea>
    {:else}
        <input
            {type}
            class="w-full border rounded p-2 {errorMsg ? 'border-red-600' : ''}"
            {required}
            on:change={valueChanged}
            maxlength={maxLength || undefined}
            placeholder={name + (required ? "*" : "")}
            value={value || ""}
        />
    {/if}
    <small class="text-red-600">{errorMsg}</small>
</div>
