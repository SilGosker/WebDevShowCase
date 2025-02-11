<script lang="ts">
	import Navigation from "./Components/Navigation/Navigation.svelte";
    import Router from "svelte-spa-router";
    import Routes from "./Routes";
    import GdprPopup from "./Components/Gdpr/GdprPopup.svelte";
</script>

<Navigation />

<Router routes="{Routes}" />

<GdprPopup />