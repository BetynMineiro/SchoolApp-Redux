import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { UserService } from 'app/core/user/user.service';
import { catchError, map, mergeMap, of } from 'rxjs';
import { UpdateUserInput } from '../user.types';
import * as UserActions from './user.actions';
@Injectable()
export class UserEffects {
    constructor(
        private actions$: Actions,
        private userService: UserService
    ) {}

    loadUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.loadUser),
            mergeMap(() =>
                this.userService.get().pipe(
                    map((response) =>
                        UserActions.loadUserSuccess({ user: response.data })
                    ),
                    catchError((error) =>
                        of(UserActions.loadUserFailure({ error }))
                    )
                )
            )
        )
    );

    updateUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.updateUser),
            mergeMap(({ user }) =>
                this.userService.update(new UpdateUserInput(user)).pipe(
                    map(() => UserActions.updateUserSuccess({ user })),
                    catchError((error) =>
                        of(UserActions.updateUserFailure({ error }))
                    )
                )
            )
        )
    );

    createUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.createUser),
            mergeMap(({ user }) =>
                this.userService.create(user).then(
                    (response) =>
                        UserActions.createUserSuccess({
                            created: response.data,
                        }),
                    (error) => UserActions.createUserFailure({ error })
                )
            )
        )
    );

    loadUserList$ = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.loadUserList),
            mergeMap(({ filterText, pageNumber, pageSize }) =>
                this.userService.getList(filterText, pageNumber, pageSize).then(
                    (response) =>
                        UserActions.loadUserListSuccess({
                            result: response.data,
                        }),
                    (error) => UserActions.loadUserListFailure({ error })
                )
            )
        )
    );

    deleteUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.deleteUser),
            mergeMap(({ id }) =>
                this.userService.delete(id).then(
                    () => UserActions.deleteUserSuccess({ id }),
                    (error) => UserActions.deleteUserFailure({ error })
                )
            )
        )
    );

    loadUserById$ = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.loadUserById),
            mergeMap(({ id }) =>
                this.userService.getUserById(id).then(
                    (response) =>
                        UserActions.loadUserByIdSuccess({
                            user: response.data,
                        }),
                    (error) => UserActions.loadUserByIdFailure({ error })
                )
            )
        )
    );
}
