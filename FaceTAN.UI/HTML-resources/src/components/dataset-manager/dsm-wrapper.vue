<template>
    <div>
        <span >DataSet Manager</span>
        <transition name="component-fade" mode="out-in">
            <component v-if="seen" v-bind:is="this.list"></component>
            <component v-else v-bind:is="this.edit"></component>
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

const AppProps = Vue.extend({
  data: {
  }
})

@Component({
    components: { 
        dsmList, 
        dsmEdit
    },
})

export default class dsmWrapper extends AppProps {
    seen: boolean = true

    list = dsmList;
    edit = dsmEdit;
    
    sendalert(){
        window.alert(this.seen);
    }
}
</script>

<style lang="scss">

</style>