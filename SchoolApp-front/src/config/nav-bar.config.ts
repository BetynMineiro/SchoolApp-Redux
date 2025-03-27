import { Menu } from '@fuse/components/navigation/menu.model';

export const NAVBAR = [
    {
        name: 'Home',
        goTo: 'home',
        icon: 'heroicons_outline:home',
    },
    {
        name: 'Users',
        goTo: 'admin/user',
        icon: 'heroicons_outline:users',
        profile: ['Admin'],
    },
    {
        name: 'Schools',
        goTo: 'admin/school',
        icon: 'mat_outline:queue_music',
        profile: ['Admin', 'Judge'],
    },
    {
        name: 'My School',
        icon: 'heroicons_outline:musical-note',
        profile: ['SchoolManager'],
        goTo: 'mySchool',
    },
] as Menu[];
