<template>
    <div class="Test-Explorer">
        <span>Test Explorer</span>

        <table>
            <tr>
                <th>API</th>
                <th>Test Guid</th>
                <th>Source Subset</th>
                <th>Target Subset</th>
                <th></th>
            </tr>
        
            <tr v-for="test in getTests()" :key="test.guid" :id="test.guid">
                <td>{{ apiName[test.api] }}</td>
                <td><span>{{ test.guid }}</span></td>
                <td>{{ getSubset(test.sourceGuid).name }}</td>
                <td>{{ getSubset(test.targetGuid).name }}</td>
                <td>
                    <button class="clear" v-if="test.resultGuid != null && test.result.length > 0" v-on:click="clearTest(test.guid)">Clear</button>
                    <button class="clear" v-else-if="test.resultGuid != null" disabled>Loading</button>
                    <button class="run" v-else v-on:click="runTest(test.guid)">Run</button>
                </td>
            </tr>
        </table>
    </div>
</template>

<script lang="ts">
// Import Vue 
import Vue from 'vue';
import Vuex from 'vuex';
import Component from "vue-class-component";
import GlobalHelpers from '../../GlobalHelpers';

let Global = new GlobalHelpers();

// Import Data Models
import * as T from "../../models/models";

@Component({
    components: { 
    }
})

export default class tmList extends Vue {

    apiName: string[] = ["Amazon", "Azure"];

    mounted() {
        this.getTests().forEach(test => {
            if(test.resultGuid != null) this.mountOutput(test);
        });
    }

    getSubset(subsetGuid: string): T.ImageList {
        return this.$store.getters.getSubset(subsetGuid)[0];
    }
    getTests(): T.Test[] {
        return this.$store.getters.getTests;
    }

    runTest(testGuid: string): void {
        let testObject: T.Test = this.$store.getters.getTest(testGuid)[0];

        testObject.resultGuid = Global.newGuid();
        
        this.$store.dispatch('requestTestResult', testObject).then(() => {
            this.mountOutput(testObject);
        });

        
    }

    clearTest(testGuid: string):void {
        let testObject: T.Test = this.$store.getters.getTest(testGuid)[0];

        document.getElementById(testObject.resultGuid).remove();
        testObject.resultGuid = null;
        testObject.result = "";

    }

    mountOutput(testObject: T.Test): void {
        let outputElement: HTMLElement = this.buildOutputHTML(testObject.resultGuid, testObject.result).getElementById(testObject.resultGuid);
        let testElement: HTMLElement = document.getElementById(testObject.guid);

        testElement.parentNode.insertBefore(outputElement, testElement.nextSibling);
    }

    buildOutputHTML(resultGuid: string, textareaContent?: string): HTMLDocument{

        let markup: string ='   <table><tr id="' + resultGuid + '" class="Test-Output">' +
                            '       <td colspan="5">' +
                            '           <h2>Output:</h2>' +
                            '           <textarea cols=50>' +
                            '           </textarea>' +
                            '       </td>' +
                            '   </tr></table>';

        var obj = JSON.parse(textareaContent);
        var pretty = JSON.stringify(obj, undefined, 4);
        let dom: HTMLDocument = new DOMParser().parseFromString(markup, "text/html");
        dom.querySelector("textarea").value = JSON.parse(JSON.stringify(pretty, undefined, 4));
        
        return dom;
    }
}
</script>

<style lang="scss" >
    @import url('https://fonts.googleapis.com/css?family=Roboto:500');

    .Test-Explorer {
        span {
            font-size: 25px;
        }
        table {
            padding: 30px 0px 40px 0px;
            min-width: 95%;
            margin-right: 5%;

            tr {
                
                text-align: left;

                th {
                    font-family: 'Segoe UI', sans-seif;
                    font-size: 22px;
                    font-weight: 400;

                    &:nth-child(1) {
                        width: 20%;
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
                    &:nth-child(5) {

                        text-align: center;

                        button {

                            border: 0 !important;
                            outline: 0 !important ;
                            border-radius: 3px;
                            font-family: 'Segoe UI';
                            font-size: 18px;
                            cursor: pointer;
                            padding: 7px 10px;
                            color: black !important;

                            &.run {
                                background-color: #2ecc71;

                                &:hover{
                                    background-color: #31d878;
                                }
                            }
                            &.clear {
                                background-color: #f39c12;

                                &:hover {
                                    background-color: #fbac2f;
                                }
                            }
                        }
                    }
                    span {   
                        font-size: 20px; 
                        width: 80%;
                        position: absolute;
                        top: 25px;
                        left: 0;
                        right: 0;
                        white-space: nowrap;
                        overflow: hidden;
                        text-overflow: ellipsis;
                    }
                }
            }
            .Test-Output {

                border-radius: 4px;
                background-color: #f1f1f1;

                td {
                    padding: 20px 30px;

                    h2{
                        font-weight: 400;
                        font-size: 16px;
                        margin-top: 0;
                    }
                    textarea {
                        overflow-y: scroll;
                        resize: none;
                        width: calc(100% - 30px);
                        min-height: 40vh;
                        color: #f1f1f1;
                        background-color: #2b2f3e;
                        font-family: 'Roboto', sans-serif;
                        font-weight: 500;
                        padding: 15px;
                        font-size: 15px;
                        letter-spacing: 1.5px;
                        outline: none;
                    }
                }
            }
            
        }
    }
</style>
    