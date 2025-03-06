<script lang="ts">
    import { UserContext } from "../../Functions/UserContext";

    let isLoggedIn = false;
    
    UserContext.addEventListener("authChanged", (loggedIn) => {
        isLoggedIn = loggedIn;
    });

    const userContext = new UserContext();

    function logOut() {
        userContext.clear();
    }
</script>

<div class="px-4 mx-auto sm:px-6">
    <div class="relative pt-6 pb-8">
        <nav
            class="relative flex items-center content-center justify-between"
            aria-label="Global"
        >
            <div class="mr-6 flex items-center self-start">
                <a href="/">
                    <img
                        alt="AI gegenereerd logo voor Sil Gosker"
                        class="w-auto h-8 md:h-16"
                        src="/images/logo_original.webp"
                    />
                </a>
            </div>
            <div class="md:px-24 flex flex-wrap justify-around md:justify-between w-full">
                <a
                    href="/"
                    class="text-xl md:text-base font-normal text-gray-500 list-none hover:text-gray-900"
                >
                    Over mij
                </a>
                <a
                    href="/#/contact"
                    class="text-xl md:text-base font-normal text-gray-500 list-none hover:text-gray-900"
                >
                    Contact
                </a>
                {#if isLoggedIn}
                    <a
                        href="/#/plants/"
                        class="text-xl md:text-base font-normal text-gray-500 list-none hover:text-gray-900"
                    >
                        Uw planten
                    </a>
                    <a
                        href="/"
                        on:click={logOut}
                        class="text-xl md:text-base font-normal text-gray-500 list-none hover:text-gray-900"
                    >
                        Uitloggen
                    </a>
                {:else}
                    <a
                        href="/#/account/login"
                        class="text-xl md:text-base font-normal text-gray-500 list-none hover:text-gray-900"
                    >
                        Inloggen
                    </a>
                {/if}
            </div>
        </nav>
    </div>
</div>
