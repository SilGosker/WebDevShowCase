<script>
    import GlideShow from "../../Components/SlideShow.svelte";
    import Slide from "../../Components/Slide.svelte";
    import WhoAmI from "./WhoAmI.svelte";
    import Skills from "./Skills.svelte";
</script>

<div class="flex flex-wrap">

    <div class="w-full sm:w-1/2">

        <h1 class="text-center text-3xl w-full text-gray-900">Sil Gosker</h1>

        <hr class="h-px my-8 bg-gray-200 border-0">

        <div class="p-4">
            <GlideShow>

                <Slide>
                    <img class="h-64" src="/images/photo_pretty.jpeg"/>
                </Slide>

                <Slide>
                    <img class="h-64" src="/images/photo_professional.jpg"/>
                </Slide>

                <Slide>
                    <img class="h-64" src="/images/photo_propedeuse.jpeg"/>
                </Slide>

                <Slide>
                    <img class="h-64" src="/images/photo_moped.jpeg"/>
                </Slide>

                <Slide>
                    <img class="h-64" src="/images/photo_push.jpeg"/>
                </Slide>

                <Slide>
                    <img class="h-64" src="/images/photo_brother.jpeg"/>
                </Slide>

                <Slide>
                    <img class="h-64" src="/images/photo_water.jpeg"/>
                </Slide>

            </GlideShow>
        </div>
    </div>

    <div class="w-full sm:w-1/2">
        <h2 class="text-center text-3xl w-full text-gray-900">Wie ben ik?</h2>

        <hr class="h-px my-8 bg-gray-200 border-0">
        <WhoAmI />
    </div>

    <hr class="h-px my-8 bg-gray-200 border-0">

    <Skills />
</div>