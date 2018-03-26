<template>
    
    <div class="container">
        <app-nav></app-nav>

        <div class="app-content">
            <router-view></router-view>
        </div>
    </div>
</template>

<script lang="ts">
import AppNav from './app-nav.vue';

export default {
    components: { AppNav }
}
</script>

<style lang="scss">
html {
  min-height: 100%;
}
body {
    margin: 0;
}
.container{
    display: inline-block;
    min-height: 100%;
    min-width: 100%;

    .app-content{
        display: inline-block;
        float: left;
        margin-left: 40%;

        font-family: 'Segoe UI';
        font-size: 2em;
        letter-spacing: 1px;
        font-weight: 200;  
        padding-top: 50px;
    }

    img{
        padding-top: 60px;
    }
}
</style>