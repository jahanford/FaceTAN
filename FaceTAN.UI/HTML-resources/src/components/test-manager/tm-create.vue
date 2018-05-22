<template>
    <table class="Create-Table">
        <h1>Create Test</h1>
        <tr>
            <td>
                <h1>Source Subset</h1>

                <select id="selectSource" v-if="getSubsets().length >= 1">
                    <option v-for="subset in getSubsets()" :key="subset.guid" :value="subset.guid">{{subset.name}}</option>
                </select>
                <span v-else class="Warn">Warn: No Subsets Exist</span>
            </td>

            <td>+</td>
            <td>
                <h1>Target Subset</h1>

                <select id="selectTarget" v-if="getSubsets().length >= 1">
                    <option v-for="subset in getSubsets()" :key="subset.guid">{{subset.name}}</option>
                </select>
                <span v-else class="Warn">Warn: No Subsets Exist</span>
            </td>

            <!-- Submit Button -->
            <td>
                <button v-if="getSubsets().length >= 1" v-on:click="createTest()">Create</button>
                <button v-else type="submit" disabled>Create</button>
            </td>
        </tr>
    </table>
</template>

<script lang="ts">
// Import Vue 
import Vue from 'vue';
import Vuex from 'vuex';
import Component from "vue-class-component";

// Import Data Models
import * as T from "../../models/models";

@Component({
    components: { 
    }
})

export default class tmCreate extends Vue {
    getSubsets() {
        return this.$store.getters.getSubsets;
    }

    createTest() {
        let source: string = (<HTMLInputElement>document.getElementById("selectSource")).value;
        let target: string = (<HTMLInputElement>document.getElementById("selectTarget")).value;
        //let target= document.getElementById("selectTarget").value;  
        console.log("CREATED TEST Source: " + source + " Target: " + target);
    }
}
</script>

<style lang="scss">
.Create-Table {
    
    font-size: 15px;
    text-align: left;
    background-color: #f1f1f1;
    padding: 30px 50px 40px 50px;
    border-radius: 10px;
    min-width: 95%;
    margin: 50px 0;

    h1 {
        font-weight: 600;
        font-family: 'Segoe UI';
        color: #2a3c4d;
    }

    tr {
        h1{
            font-weight: 300;
        }
        td:nth-child(1) {
            width: 32.5%;
        }
        td:nth-child(2) {
            
            width: 15%;
            text-align: center;
            font-size: 100px;
            color: #c3c3c3;
            font-weight: 100;
            font-family: initial;

        }
        td:nth-child(3) {
            width: 32.5%;
        }
        td:nth-child(4) {
            width: 20%;
            text-align: right;

            button {
                background-color: #3498db;
        
                height: 55px;
                width: 110px;
                border: none;
                border-radius: 2px;

                font-family: 'Segoe UI';
                font-size: 20px;
                cursor: pointer;

                &:disabled {
                    background-color: #8a8a8a;
                    color: white;
                    cursor: auto;
                }
            }
        }
                        
    }
}

.Warn {
    color: red;
    font-weight: 400;
    font-size: 13px;
}

/* DROP DOWN */
select {
    width: 100%;
    height: 55px;
    background-color: #2a3c4d;

    border: none;
    color: #fbfbfb;
    margin: 5px auto;
    padding-left: 35px;
    padding-right: 15px;
    font-family: 'Segoe UI';

    border-radius: 3px;
    outline: none;

}
option {
    color: #555;
    background-color: #FFF;
    padding-left: 55px;
    padding-top: 10px;
    padding-bottom: 10px;

    &:hover {
        background-color: red;
    }
}
</style>