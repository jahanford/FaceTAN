<template>
    <div>
        <span>Test Manager</span>

        <tmCreate></tmCreate>
        <tmList></tmList>

    </div>
</template>

<script lang="ts">
// Import Vue 
import Vue from 'vue';
import Vuex from 'vuex';
import Component from "vue-class-component";

// Import Required Components
import tmCreate from './tm-create.vue'
import tmList from './tm-list.vue';

// Import Data Models
import * as T from "../../models/models";

@Component({
    components: { 
        tmList, 
        tmCreate
    }
})

export default class tmWrapper extends Vue {

}
</script>

<style lang="scss">
    .bold{
        font-weight: 600;
    }
</style>