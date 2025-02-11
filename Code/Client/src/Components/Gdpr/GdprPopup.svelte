<script lang="ts">
    let showGdpr = !localStorage.getItem("cookie");

    function setCookie(accepted: boolean): void {
        const object = {
            // formats to yyyy-MM-dd
            date: new Date().toISOString().split('T')[0],
            // formats to hh:mm:ss
            time: new Date().toTimeString().split(' ')[0],
            accepted
        };

        localStorage.setItem("cookie", JSON.stringify(object));
    }

    function acceptCookies() {
        setCookie(true);
        showGdpr = false;
    }

    function declineCookies() {
        setCookie(false);
        showGdpr = false;
    }
</script>

{#if showGdpr}
    <div class="fixed bottom-0 w-full p-4">
        <div class="bg-white p-4 w-full rounded shadow-lg border flex flex-col md:flex-row content-center justify-between">
            <button on:click={acceptCookies} class="py-2 px-4 border-sky-600 bg-sky-400 text-white rounded-lg">
                Accepteer
            </button>
            <p class="px-4">
                Deze website gebruikt cookies.
                We gebruiken cookies om content te personaliseren, voor social media en het analyseren
                van verkeer op de website, advertenties.
            </p>
            <button on:click={declineCookies} class="py-2 px-4 border-gray-600 bg-gray-400 text-white rounded-lg">
                Negeren
            </button>
        </div>
    </div>
{/if}