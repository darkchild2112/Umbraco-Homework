

<template>
    <div class="alert alert-danger" role="alert">
        <p>{{ error.summary }}</p>
        <ul v-if="error.errors != null">
            <li v-bind:key="index" v-for="(value, index) in error.errors">
                {{ value }}
            </li>
        </ul>
    </div>
</template>

<script>
export default {
    name: "ErrorAlert",
    props: {
        error: Object,
    }
}
</script>