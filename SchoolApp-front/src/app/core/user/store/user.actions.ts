import { createAction, props } from '@ngrx/store';
import {
    CreateUserInput,
    CreateUserOutput,
    UpdateUserInput,
    User,
    UserQueryResult,
    UserQueryResultForList,
} from 'app/core/user/user.types';
import { Pagination } from 'config/model/app.config.model';

export const loadUser = createAction('[User] Load');
export const loadUserSuccess = createAction(
    '[User] Load Success',
    props<{ user: User }>()
);
export const loadUserFailure = createAction(
    '[User] Load Failure',
    props<{ error: any }>()
);

export const updateUser = createAction(
    '[User] Update',
    props<{ user: UpdateUserInput }>()
);
export const updateUserSuccess = createAction(
    '[User] Update Success',
    props<{ user: any }>()
);
export const updateUserFailure = createAction(
    '[User] Update Failure',
    props<{ error: any }>()
);

export const createUser = createAction(
    '[User] Create',
    props<{ user: CreateUserInput }>()
);
export const createUserSuccess = createAction(
    '[User] Create Success',
    props<{ created: CreateUserOutput }>()
);
export const createUserFailure = createAction(
    '[User] Create Failure',
    props<{ error: any }>()
);

export const loadUserList = createAction(
    '[User] Load List',
    props<{ filterText: string; pageNumber: number; pageSize: number }>()
);
export const loadUserListSuccess = createAction(
    '[User] Load List Success',
    props<{ result: Pagination<UserQueryResultForList> }>()
);
export const loadUserListFailure = createAction(
    '[User] Load List Failure',
    props<{ error: any }>()
);

export const deleteUser = createAction(
    '[User] Delete',
    props<{ id: string }>()
);
export const deleteUserSuccess = createAction(
    '[User] Delete Success',
    props<{ id: string }>()
);
export const deleteUserFailure = createAction(
    '[User] Delete Failure',
    props<{ error: any }>()
);
export const loadUserById = createAction(
    '[User] Load By Id',
    props<{ id: string }>()
);
export const loadUserByIdSuccess = createAction(
    '[User] Load By Id Success',
    props<{ user: UserQueryResult }>()
);
export const loadUserByIdFailure = createAction(
    '[User] Load By Id Failure',
    props<{ error: any }>()
);
