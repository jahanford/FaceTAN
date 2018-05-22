<template>
    <div>
        <span>Test Explorer</span>

        <table>
            <tr>
                <th>Time Stamp</th>
                <th>Test Guid</th>
                <th>Source Subset</th>
                <th>Target Subset</th>
                <th></th>
            </tr>
        
            <tr v-for="test in getTests()" :key="test.guid">
                <td>{{ test.timestamp }}</td>
                <td><span>{{ test.guid }}</span></td>
                <td>{{ getSubset(test.sourceGuid).name }}</td>
                <td>{{ getSubset(test.targetGuid).name }}</td>
                <td>
                    <button>Run</button>
                </td>
            </tr>

            <tr id="Test-Output">
                <td colspan="5">Output:</td>
            </tr>
        </table>
    </div>
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

export default class tmList extends Vue {

    getSubset(subsetGuid: string): T.ImageList {
        return this.$store.getters.getSubset(subsetGuid)[0];
    }
    getTests(): T.Test[] {
        return this.$store.getters.getTests;
    }
}
</script>

<style lang="scss" scoped>
    span {
        font-size: 25px;
    }
    table {
        padding: 30px 0px 40px 0px;
        min-width: 95%;
        margin-right: 5%;


        tr {
            
            text-align: left;

            &#Test-Output {
                background-color: #f1f1f1;
            }

            th {
                font-family: 'Segoe UI', sans-seif;
                font-size: 22px;
                font-weight: 400;

                &:nth-child(1) {
                    width: 15%;
                }

                &:nth-child(2) {
                    width: 30%;
                }

                &:nth-child(3) {

                }
            }
            
            td {
                font-family: 'Segoe UI', sans-seif;
                font-size: 20px;
                position: relative;
                padding: 20px 0;

                &:nth-child(2) {
                    display: block;
                }

                span {    
                    width: 80%;
                    position: absolute;
                    top: 20px;
                    left: 0;
                    right: 0;
                    white-space: nowrap;
                    overflow: hidden;
                    text-overflow: ellipsis;
                }
            }
        }
    }
</style>
    