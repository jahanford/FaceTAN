<template>
    <ul>
        <li v-for="subset in store.getters.getSubsetByGuid("123")">
            <span>{{subset.name}}</span>
            <span>{{subset.guid}}</span>
        </li>
    </ul>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from "vue-class-component"
import * as T from "../../models/models";

@Component({
    name: "dsmEdit"  
})

export default class dsmEdit extends Vue {
    subset: T.ImageList = $store getSubsetByGuid
}
</script>