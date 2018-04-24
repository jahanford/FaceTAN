import Vue from 'vue'
import Vuex, { GetterTree } from 'vuex'
import { MutationTree, ActionTree,  } from 'vuex'
import * as T from '../models/models'

Vue.use(Vuex)

interface State {
    
    subsets: T.ImageList[];
}

const getters: GetterTree<State, any> = {
    
    
    getSubsetByGuid: (State) => (id: String) => {
    state.subsets.forEach( subset => {
        if(subset.guid == id)return subset;
    });
    }    
    

}

const mutations: MutationTree<State> = {

    addSubset (State, payload: T.ImageList) {
        State.subsets.push(payload);
    },

    removeSubset (State, guid) {
        state.subsets.forEach( (subset, index) => {
            if(subset.guid === guid){
                state.subsets.splice(index, 1);
            }
        });
    }

}

const actions: ActionTree<State,any> = {

    addSubset (State) {
        State.state.subsets
    }


}

const state: State = {

    subsets: [
        //
        {   name: "Subset One", 
            guid: "43b4290e-fddb-4d97-9b9f-6f961987d5fe", 
            imageStore: [
                    {   name: "Selected Image", 
                        url: ""
                    }
            ]
        },
        //
        {   name: "Subset Two", 
            guid: "fed48f8b-7c8d-496f-b844-e383d970187a", 
            imageStore: [
                    {   name: "hi", 
                        url: ""
                    }
            ]
        },
        //
        {   name: "Another Subset", 
            guid: "63f558dd-c3c3-47c9-a82e-e67d41aaa0a6", 
            imageStore: [
                    {   name: "hi", 
                        url: ""
                    }
            ]
        },
        //
        {   name: "Final Subset", 
            guid: "0c7f3967-dae1-42f7-b1a0-a093b0a63a7a", 
            imageStore: [
                    {   name: "hi", 
                        url: ""
                    }
            ]
        },
    ]
}

export default new Vuex.Store<State>({
    state,
    mutations,
    getters,
    actions
});
