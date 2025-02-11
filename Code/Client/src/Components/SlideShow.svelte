<script>
    import Glide from "@glidejs/glide";
    import {onMount} from "svelte";

    onMount(() => {
        const glide = new Glide(".glide");
        glide.mount();
    });
</script>

<div class="glide">
    <div class="glide__track" data-glide-el="track">
        <ul class="glide__slides">
            <slot/>
        </ul>
    </div>

    <div class="mt-4 glide__arrows flex justify-between" data-glide-el="controls">
        <button class="glide__arrow glide__arrow--left border text-gray-900 ml-16 px-4 py-2 shadow-lg rounded"
                data-glide-dir="<">Vorige
        </button>
        <button class="glide__arrow glide__arrow--right border text-gray-900 mr-16 px-4 py-2 shadow-lg rounded"
                data-glide-dir=">">Volgende
        </button>
    </div>

</div>