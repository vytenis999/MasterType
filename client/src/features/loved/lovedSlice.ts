import { createAsyncThunk, createSlice, isAnyOf } from "@reduxjs/toolkit";
import agent from "../../app/api/agent";
import { getCookie } from "../../app/util/util";
import { Loved } from "../../app/models/loved";

interface LovedState {
    loved: Loved | null
    status: string;
}

const initialState: LovedState = {
    loved: null,
    status: 'idle'
}

export const fetchLovedAsync = createAsyncThunk<Loved>(
    'loved/fetchLovedAsync',
    async (_, thunkAPI) => {
        try {
            console.log("Trying to get loved ones")
            return await agent.Loved.get()
        } catch (error: any) {
            return thunkAPI.rejectWithValue({error: error.data})
        }
    },
    {
        condition: () => {
            if (!getCookie('buyerId')) return false;
        }
    }
)

export const addLovedItemAsync = createAsyncThunk<Loved, {productId: number}>(
    'loved/addLovedItemAsync',
    async ({productId}, thunkAPI) => {
        try {
            return await agent.Loved.addItem(productId)
        } catch (error: any) {
            return thunkAPI.rejectWithValue({error: error.data})
        }
    }
)

export const removeLovedItemAsync = createAsyncThunk<void, {productId: number}>(
    'loved/removeLovedItemAsync',
    async ({productId}, thunkAPI) => {
        try {
            await agent.Loved.removeItem(productId);
        } catch (error: any) {
            return thunkAPI.rejectWithValue({error: error.data})
        }
    }
)

export const lovedSlice = createSlice({
    name: 'loved',
    initialState,
    reducers: {
        setLoved: (state, action) => {
            state.loved = action.payload
        },
        clearLoved: (state) => {
            state.loved = null;
        }
    },
    extraReducers: (builder => {
        builder.addCase(addLovedItemAsync.pending, (state, action) => {
            state.status = 'pendingAddItem' + action.meta.arg.productId;
        });
        builder.addCase(removeLovedItemAsync.pending, (state, action) => {
            state.status = 'pendingRemoveItem' + action.meta.arg.productId;
        });
        builder.addCase(removeLovedItemAsync.fulfilled, (state, action) => {
            const {productId} = action.meta.arg;
            const itemIndex = state.loved?.items.findIndex(i => i.productId === productId);
            if (itemIndex === -1 || itemIndex === undefined) return;
            state.loved?.items.splice(itemIndex, 1);
            state.status = 'idle';
        });
        builder.addCase(removeLovedItemAsync.rejected, (state, action) => {
            state.status = 'idle';
            console.log(action.payload);
        });
        builder.addMatcher(isAnyOf(addLovedItemAsync.fulfilled, fetchLovedAsync.fulfilled), (state, action) => {
            state.loved = action.payload;
            state.status = 'idle';
        });
        builder.addMatcher(isAnyOf(addLovedItemAsync.rejected, fetchLovedAsync.rejected), (state, action) => {
            state.status = 'idle';
            console.log(action.payload);
        });
    })
})

export const {setLoved, clearLoved } = lovedSlice.actions;