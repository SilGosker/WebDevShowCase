<script lang="ts">
    export let title: string;
    export let description: string = undefined;
    export let link: string = undefined;
</script>

<div
    class="m-3 max-w-sm p-6 bg-white border border-gray-200 rounded-lg shadow-sm"
>
    <h5 class="mb-2 text-2xl font-bold tracking-tight text-gray-900">
        {title}
    </h5>
    {#if description}
        <p class="mb-3 font-normal text-gray-700">
            {description}
        </p>
    {/if}

    <slot />

    {#if link}
        <a
            href="/#/{link}"
            class="self-end inline-flex items-center px-3 py-2 text-sm font-medium text-center text-white bg-sky-400 rounded-lg hover:bg-sky-600 focus:ring-4 focus:outline-none focus:ring-blue-300"
        >
            Meer informatie
            <svg
                class="rtl:rotate-180 w-3.5 h-3.5 ms-2"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 14 10"
            >
                <path
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M1 5h12m0 0L9 1m4 4L9 9"
                />
            </svg>
        </a>
    {/if}
</div>
