<template>
    <input class="btn btn-primary" type="submit" :value="text" :disabled="enabled" />
</template>

<script>
    export default {
        name: "Button",
        props: {
            text: String,
            enabled: Boolean
        },
    }
</script>