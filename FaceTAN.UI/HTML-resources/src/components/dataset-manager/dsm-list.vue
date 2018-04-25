<template>
    <ul>
        <li v-for="subset in getSubsets()" :key="subset.guid">
            <div v-on:click="editSubset(subset.guid)" class="DS-Thumb">DS</div>
            <span>{{subset.name}}</span>
            <span>{{subset.guid}}</span>
        </li>

        <li v-on:click="newSubset()">
            <div class="DS-Add">+</div>
            <span>New Subset</span>
        </li>

        <button v-on:click="$store.commit('reverse')">Reverse</button>
        
    </ul>
</template>

<script lang="ts">
import Vue from 'vue'
import Vuex from 'vuex'
import Component from "vue-class-component"
import * as T from "../../models/models";

@Component
export default class dsmList extends Vue {

    newGuid(): string {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            const r = Math.random() * 16 | 0, v = c === 'x' ? r : ( r & 0x3 | 0x8 );
            return v.toString(16);
        });
    }

    getSubsets() {
        return this.$store.getters.getSubsets;
    }
    newSubset() {
        let generatedGuid = this.newGuid();

        this.$store.commit('addSubset', {guid: generatedGuid , name: "New Subset", imageStore: []});
        this.editSubset(generatedGuid);
    }

    editSubset(guid: string) {
        this.$store.commit('updateView', {edit: true, guid: guid});
    }

}
</script>

<style lang="scss">
ul{
    list-style: none;
    margin: 0;
    padding: 0;
    padding-top: 3%;

    li{
        padding: 1.5rem;
        width: 16.66666667%;
        float: left;
        display: table;

        .DS-Thumb{
            width: 15rem;
            height: 15rem;
            margin: 0 auto;
            background-color: #FBFBFB;
            border-bottom: 2px solid #EAEAEA;
            line-height: 15rem;
            text-align: center;
            font-family: 'Segoe UI';
            font-weight: 500;
            color: #2B2F3E;
            font-size: 300%;
            cursor: pointer;
        }

        .DS-Thumb:hover{
            background-color: #eeedee;
            
        }

        .DS-Add{
            width: 15rem;
            height: 15rem;
            margin: 0 auto;
            background-color: #ffffff;
            border: 4px dashed #eeedee;
            line-height: 15rem;
            text-align: center;
            font-family: 'Segoe UI';
            font-weight: 500;
            color: #2a3c4d;
            font-size: 300%;
            cursor: pointer;
        }

        span{
            display: block;
            width: 100%;

            
        }
        span:nth-child(3) {
            font-size: 60%;
            color: #c3c3c3;
        }
    }
}

</style>
    