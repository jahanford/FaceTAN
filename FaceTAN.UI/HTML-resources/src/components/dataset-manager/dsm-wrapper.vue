<template>
    <div>
        <span>DataSet Manager</span>
        <span v-if="this.$store.state.dsmView.edit" class="bold"> - Subset Edit</span>

        <transition name="component-fade" mode="out-in">
            <component v-if="this.$store.state.dsmView.edit" v-bind:is="this.edit"></component>
            <component v-else v-bind:is="this.list"></component>
        </transition>

    </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Vuex from 'vuex'
import Component from "vue-class-component"
import dsmList from './dsm-list.vue'
import dsmEdit from './dsm-edit.vue'
import * as T from "../../models/models";



@Component({
    components: { 
        dsmList, 
        dsmEdit
    }
})

export default class dsmWrapper extends Vue {
    seen: boolean = true

    list = dsmList;
    edit = dsmEdit;
    
    sendalert(){
        window.alert(this.seen);
    }
}
</script>

<style lang="scss">
    .bold{
        font-weight: 600;
    }
</style>