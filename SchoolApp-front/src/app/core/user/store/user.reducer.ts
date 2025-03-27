import { createReducer, on } from '@ngrx/store';
import * as UserActions from './user.actions';
import { initialState } from './user.state';

export const userReducer = createReducer(
    initialState,
    on(UserActions.loadUser, (state) => ({ ...state, loading: true })),
    on(UserActions.loadUserSuccess, (state, { user }) => ({
        ...state,
        user,
        loading: false,
    })),
    on(UserActions.loadUserFailure, (state, { error }) => ({
        ...state,
        error,
        loading: false,
    })),

    on(UserActions.updateUser, (state) => ({ ...state, loading: true })),
    on(UserActions.updateUserSuccess, (state, { user }) => ({
        ...state,
        user,
        loading: false,
    })),
    on(UserActions.updateUserFailure, (state, { error }) => ({
        ...state,
        error,
        loading: false,
    })),

    on(UserActions.createUser, (state) => ({ ...state, loading: true })),
    on(UserActions.createUserSuccess, (state) => ({
        ...state,
        loading: false,
    })),
    on(UserActions.createUserFailure, (state, { error }) => ({
        ...state,
        error,
        loading: false,
    })),

    on(UserActions.loadUserList, (state) => ({ ...state, loading: true })),
    on(UserActions.loadUserListSuccess, (state, { result }) => ({
        ...state,
        userList: result,
        loading: false,
    })),
    on(UserActions.loadUserListFailure, (state, { error }) => ({
        ...state,
        error,
        loading: false,
    })),

    on(UserActions.deleteUser, (state) => ({ ...state, loading: true })),
    on(UserActions.deleteUserSuccess, (state, { id }) => ({
        ...state,
        userList: {
            ...state.userList,
            items: state.userList?.items?.filter((u) => u.id !== id) || [],
        },
        loading: false,
    })),
    on(UserActions.deleteUserFailure, (state, { error }) => ({
        ...state,
        error,
        loading: false,
    })),
    on(UserActions.loadUserById, (state) => ({ ...state, loading: true })),
    on(UserActions.loadUserByIdSuccess, (state, { user }) => ({
        ...state,
        user,
        loading: false,
    })),
    on(UserActions.loadUserByIdFailure, (state, { error }) => ({
        ...state,
        error,
        loading: false,
    }))
);
