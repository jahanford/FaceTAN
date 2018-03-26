<template>
    <nav>

        <div class="logo">
            Project<span class="bold">FaceTAN</span>
        </div>
        <ul class="">
            <router-link class="nav-item" to="dataset-manager" tag="li">
                <span>DataSet Manager</span>
            </router-link>
            <router-link class="nav-item" to="amazon" tag="li">
                    <span>Amazon</span>
            </router-link>
            <router-link class="nav-item" to="azure" tag="li">
                    <span>Azure Face API</span>
            </router-link>
            <router-link class="nav-item" to="animetrics" tag="li">
                    <span>Animetrics</span>
            </router-link>
            <router-link class="nav-item" to="lambda-labs" tag="li">
                    <span>Lambda Labs</span>
            </router-link>
            <router-link class="nav-item" to="sky-biometry" tag="li">
                    <span>SkyBiometry</span>
            </router-link>
        </ul>
    </nav>
</template>

<script lang="ts">
import Vue from 'vue';

export default class AppNav extends Vue{
   name: string = 'AppNav'
};
</script>

<style lang="scss">
    nav{
        display: inline-block;
        float: left;
        position: fixed;
        width:  25%;
        min-width: 300px;
        max-width: 400px;
        min-height:  100vh;
        background-color:  #eeedee;
        overflow: hidden;

        .logo{
            display: block;
            padding: 30px 0px 100px 30px;

            color: #2a3c4d;
            font-family: 'Segoe UI';
            font-size: 1.8em;
            
            font-weight: 200;

            .bold{
                font-weight: 600;
                padding-left: 8px;
            }
        }

        ul{
            list-style: none;
            margin: 0;
            padding: 0;

            li{
                width: 100%;
                padding: 18px 0;
                color: #000;
                font-family: 'Segoe UI';
                font-size: 1.2em;
                letter-spacing: 1px;
                font-weight: 200;              

                    span{
                        margin-left: 80px;
                    }
            }

            li:hover{
                background-color: #e6e6e6;
            }

            .router-link-exact-active{
                border-left: solid 5px #2a3c4d;
                
                span{
                    margin-left: 75px;
                }
            }
        
        }
    }
</style>