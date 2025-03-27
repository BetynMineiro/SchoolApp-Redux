import { createFeatureSelector, createSelector } from '@ngrx/store';
import { UserState } from './user.state';

export const selectUserState = createFeatureSelector<UserState>('user');

export const selectCurrentUser = createSelector(
    selectUserState,
    (state) => state.user
);
export const selectUserLoading = createSelector(
    selectUserState,
    (state) => state.loading
);
export const selectUserError = createSelector(
    selectUserState,
    (state) => state.error
);
export const selectUserList = createSelector(
    selectUserState,
    (state) => state.userList
);
export const selectUserDetail = createSelector(
    selectUserState,
    (state) => state.user
);
