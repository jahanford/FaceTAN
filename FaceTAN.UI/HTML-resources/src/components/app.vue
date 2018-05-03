<template>
    
    <div class="container">
        <app-nav></app-nav>

        <div class="app-content">
            <router-view></router-view>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from "vue-class-component"
import AppNav from './app-nav.vue';

//
// Working Object Binding Example
//
declare var CefSharp: CefSharp;

interface CefSharp{
    BindObjectAsync(x: string, y: string): any;
}

declare var boundDataSet: DataSet;

interface DataSet{
    hello(x: string): any
}

(async () => {
    await CefSharp.BindObjectAsync("boundDataSet", "boundDataSet");

    boundDataSet.hello('CefSharp').then(function (res: string)
	{
        console.log("CEF Response: " + res);
    });

})();

@Component({
    name: 'app',
    components: { AppNav }
})

export default class App extends Vue {
    
}
</script>


<style lang="scss">

/********************************
BREAKPOINT MIXINS
********************************/

//Map of breakpoints
$breakpoints: (
  tablet: 64rem,
  laptop: 70rem,
  tv: 80rem
);

@mixin break($size) {
    @media (max-width: map-get($breakpoints, $size)) { @content ; }
}

@mixin breakh($size){
  @media (max-height: map-get($breakpoints, $size)) { @content ; }
}


@mixin orientation($or) {
    @media (orientation: $or) { @content ; }
}

html {
  min-height: 100%;
}
body {
    margin: 0;
}
.container{
    display: inline-block;
    min-height: 100%;
    min-width: 100%;

    .app-content{
        
        font-family: 'Segoe UI';
        font-size: 2em;
        letter-spacing: 1px;
        font-weight: 200;  
        margin-top: 5%;

        margin-left: 25%;

        @include break(laptop){
            margin-left: 30%;
        }

        @include break(tablet) {
            margin-left: 35%;
        }
    }

    img{
        padding-top: 60px;
    }
}
</style>