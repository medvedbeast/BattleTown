using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Randomizer
{
    public class Controller
    {
        public Generator<System.Type> Behaviour = new Generator<System.Type>(new List<Item<System.Type>> {
                new Item<System.Type>
                {
                    Content = typeof(Assaulter),
                    Weight = 3
                },
                new Item<System.Type>
                {
                    Content = typeof(Destroyer),
                    Weight = 4
                },
                new Item<System.Type>
                {
                    Content = typeof(Hunter),
                    Weight = 2
                },
                new Item<System.Type>
                {
                    Content = typeof(Support),
                    Weight = 1
                }
            });

        public Generator<System.Type> WeaponType = new Generator<System.Type>(new List<Item<System.Type>> {
                new Item<System.Type>
                {
                    Content = typeof(ArmorPenetratingCannon),
                    Weight = 1
                },
                new Item<System.Type>
                {
                    Content = typeof(DoubleBarrelCannon),
                    Weight = 1
                }
            });

        public _Loot Module = new _Loot();

        public class _Loot
        {
            public _Chassis Chassis = new _Chassis();
            public class _Chassis
            {
                public Generator<DefaultChassis> Default = new Generator<DefaultChassis>(new List<Item<DefaultChassis>> {
                    new Item<DefaultChassis>
                    {
                        Content = DefaultChassis.LEVEL_1,
                        Weight = 1
                    }
                });
            }

            public _Hull Hull = new _Hull();
            public class _Hull
            {
                public Generator<DefaultHull> Default = new Generator<DefaultHull>(new List<Item<DefaultHull>> {
                    new Item<DefaultHull>
                    {
                        Content = DefaultHull.LEVEL_1,
                        Weight = 1
                    },
                    new Item<DefaultHull>
                    {
                        Content = DefaultHull.LEVEL_2,
                        Weight = 1
                    },
                    new Item<DefaultHull>
                    {
                        Content = DefaultHull.LEVEL_3,
                        Weight = 1
                    }
                });
            }

            public _Turret Turret = new _Turret();
            public class _Turret
            {
                public Generator<DefaultTurret> Default = new Generator<DefaultTurret>(new List<Item<DefaultTurret>> {
                    new Item<DefaultTurret>
                    {
                        Content = DefaultTurret.LEVEL_1,
                        Weight = 1
                    },
                    new Item<DefaultTurret>
                    {
                        Content = DefaultTurret.LEVEL_2,
                        Weight = 1
                    },
                    new Item<DefaultTurret>
                    {
                        Content = DefaultTurret.LEVEL_3,
                        Weight = 1
                    }
                });
            }


            public _Weapon Weapon = new _Weapon();
            public class _Weapon
            {
                public Generator<ArmorPenetratingCannon> AP = new Generator<ArmorPenetratingCannon>(new List<Item<ArmorPenetratingCannon>> {
                    new Item<ArmorPenetratingCannon>
                    {
                        Content = ArmorPenetratingCannon.LEVEL_1,
                        Weight = 1
                    },
                    new Item<ArmorPenetratingCannon>
                    {
                        Content = ArmorPenetratingCannon.LEVEL_2,
                        Weight = 1
                    },
                    new Item<ArmorPenetratingCannon>
                    {
                        Content = ArmorPenetratingCannon.LEVEL_3,
                        Weight = 1
                    }
                });

                public Generator<DoubleBarrelCannon> DB = new Generator<DoubleBarrelCannon>(new List<Item<DoubleBarrelCannon>> {
                    new Item<DoubleBarrelCannon>
                    {
                        Content = DoubleBarrelCannon.LEVEL_1,
                        Weight = 1
                    },
                    new Item<DoubleBarrelCannon>
                    {
                        Content = DoubleBarrelCannon.LEVEL_2,
                        Weight = 1
                    },
                    new Item<DoubleBarrelCannon>
                    {
                        Content = DoubleBarrelCannon.LEVEL_3,
                        Weight = 1
                    }
                });
            }

            
        }

    }

}