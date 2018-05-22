<template>
    <div>

        <!-- Temp Spot Will Move To Fixed Bar Below -->
        <div class="Bottom-Bar">
            <input :value="getSubset().name" @input="updateName">
            <button v-on:click="exitEdit()">Exit</button>
        </div>

        <ul>
            <li :title="image.title" :id="image.guid" v-on:click="imageSelect(image.guid)" v-for="image in getDataSet().imageStore" :key="image.name">
                <div class="Img-Thumb" v-bind:style="{ backgroundImage: 'url(' + image.url + ')' }"></div>
                <span>{{image.name}}</span>
            </li>
        </ul>

    </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from "vue-class-component"
import * as T from "../../models/models";

@Component
export default class dsmEdit extends Vue {
    
    mounted() {
        console.log("Component Mounted");
        let selectedCollection: T.ImageElement[] = this.getSubset().imageStore;

        selectedCollection.forEach(image => document.getElementById(image.guid).classList.add('selected'))
    }

    imageSelect(guid: string) {

        let selectedCollection: T.ImageElement[] = this.getSubset().imageStore;
        let dataSet: T.ImageElement[] = this.getDataSet().imageStore;

        if(selectedCollection.filter(image => image.guid === guid)[0]){
            //Debug
            console.log("ALREADY SELECTED REMOVING " + guid);

            document.getElementById(guid).classList.remove('selected');
            selectedCollection.splice(selectedCollection.findIndex(image => image.guid === guid), 1);
        }else{
            //Debug
            console.log("ADDED TO SUBSET " + guid);

            document.getElementById(guid).classList.add('selected');
            selectedCollection.push(dataSet.find(image => image.guid === guid));
        }
    }

    getDataSet() {
        return this.$store.getters.getDataSet;
    }

    exitEdit() {
        this.$store.commit('updateView', {edit: false, guid: undefined})
    }

    updateName(e){
        this.getSubset().name = e.target.value;
    }

    getSubset() {
        let currentView: T.DsmView = this.$store.getters.getView;
        let subsetCollection: T.ImageList[] = this.$store.getters.getSubsets;

        return subsetCollection.filter(subset => subset.guid === currentView.guid)[0];
    }

}
</script>

<style lang="scss" scoped>

.Bottom-Bar {
    position: fixed;
    bottom: 0;
    width: 100%;
    height: 80px;
    margin-left: -5%;
    background-color: #2a3c4d;

    z-index: 999;

    input {
        background-color: #2a3c4d;

        margin: 20px;
        padding: 8px;
        border: 1px solid #4c6b7d;

        color: #bdbdbd;
        font-family: 'Segoe UI';
        font-size: 15px;
    }

    button {
        background-color: #3498db;
        
        height: 38px;
        width: 110px;
        border: none;
        border-radius: 2px;

        font-family: 'Segoe UI';
        font-size: 20px;
    }
}

ul {
    list-style: none;
    margin: 0;
    padding: 0;
    padding-top: 3%;

    display: inline-block;
    margin-bottom: 100px;

    .selected{
        color: white !important;
        background-color: #2a3c4d !important;
    }
    li{
        float: left;
        display: table;
        padding: 1.5rem;
        width: initial;
        cursor: pointer;

        .Img-Thumb{
            width: 10rem;
            height: 10rem;
            margin: 0 auto;
            background-size: contain;
        }

        span{
            display: block;
            width: 8rem;
            height: 4rem;
            font-size: 60%;
            /* text-align: center; */
            word-wrap: break-word;
            overflow: hidden;
            line-height: 2rem;
        }

    }

    li:hover{
        background-color: #eeedee;

        span{
            //TODO HOVER SHOW FULL NAME
        }
    }
}

</style>
