// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections
{
    /// <summary>Provides a set of tests covering the <see cref="UnmanagedValueList{T}" /> struct.</summary>
    public static class UnmanagedValueListTests
    {
        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.UnmanagedValueList()" /> constructor.</summary>
        [Test]
        public static void CtorTest()
        {
            using var valueList = new UnmanagedValueList<int>();

            Assert.That(() => valueList,
                Has.Property("Capacity").EqualTo((nuint)0)
                   .And.Count.EqualTo((nuint)0)
            );
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.UnmanagedValueList(nuint, nuint)" /> constructor.</summary>
        [Test]
        public static void CtorNUIntNUIntTest()
        {
            using (var valueList = new UnmanagedValueList<int>(5))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)5)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueList = new UnmanagedValueList<int>(5, 2))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)5)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueList<int>(5, 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );

            using (var valueList = new UnmanagedValueList<int>(0, 2))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueList<int>(0, 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.UnmanagedValueList(UnmanagedReadOnlySpan{T}, nuint)" /> constructor.</summary>
        [Test]
        public static void CtorUnmanagedReadOnlySpanNUIntTest()
        {
            using var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array.AsSpan()))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)3)
                );
            }

            using (var valueList = new UnmanagedValueList<int>(array.AsSpan(), 2))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)3)
                );
            }

            Assert.That(() => new UnmanagedValueList<int>(array.AsSpan(), 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );

            using (var valueList = new UnmanagedValueList<int>(UnmanagedArray<int>.Empty.AsSpan()))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueList = new UnmanagedValueList<int>(UnmanagedArray<int>.Empty.AsSpan(), 2))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueList<int>(UnmanagedArray<int>.Empty.AsSpan(), 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );

            using (var valueList = new UnmanagedValueList<int>(new UnmanagedArray<int>().AsSpan()))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueList = new UnmanagedValueList<int>(new UnmanagedArray<int>().AsSpan(), 2))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueList<int>(new UnmanagedArray<int>().AsSpan(), 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.UnmanagedValueList(UnmanagedArray{T}, bool)" /> constructor.</summary>
        [Test]
        public static void CtorUnmanagedArrayBooleanTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: false))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)3)
                );
            }

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)3)
                );
            }

            using (var valueList = new UnmanagedValueList<int>(UnmanagedArray<int>.Empty, takeOwnership: false))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueList = new UnmanagedValueList<int>(UnmanagedArray<int>.Empty, takeOwnership: true))
            {
                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueList<int>(new UnmanagedArray<int>(), takeOwnership: false),
                Throws.ArgumentNullException
                      .And.Property("ParamName").EqualTo("array")
            );

            Assert.That(() => new UnmanagedValueList<int>(new UnmanagedArray<int>(), takeOwnership: true),
                Throws.ArgumentNullException
                        .And.Property("ParamName").EqualTo("array")
            );
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}" /> indexer.</summary>
        [Test]
        public static void GetItemTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                Assert.That(() => valueList[1],
                    Is.EqualTo(2)
                );

                Assert.That(() => valueList[3],
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                          .And.Property("ActualValue").EqualTo((nuint)3)
                          .And.Property("ParamName").EqualTo("index")
                );
            }

            using (var valueList = new UnmanagedValueList<int>())
            {
                Assert.That(() => valueList[0],
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                          .And.Property("ActualValue").EqualTo((nuint)0)
                          .And.Property("ParamName").EqualTo("index")
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}" /> indexer.</summary>
        [Test]
        public static void SetItemTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var disposableList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                var valueList = disposableList;

                Assert.That(() => valueList[1] = 4,
                    Is.EqualTo(4)
                );

                Assert.That(() => valueList[3] = 4,
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                          .And.Property("ActualValue").EqualTo((nuint)3)
                          .And.Property("ParamName").EqualTo("index")
                );
            }

            using (var disposableList = new UnmanagedValueList<int>())
            {
                var valueList = disposableList;

                Assert.That(() => valueList[0] = 4,
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                          .And.Property("ActualValue").EqualTo((nuint)0)
                          .And.Property("ParamName").EqualTo("index")
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.Add(T)" /> method.</summary>
        [Test]
        public static void AddTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                valueList.Add(4);

                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)6)
                       .And.Count.EqualTo((nuint)4)
                );

                Assert.That(() => valueList[3],
                    Is.EqualTo(4)
                );

                valueList.Add(5);

                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)6)
                       .And.Count.EqualTo((nuint)5)
                );

                Assert.That(() => valueList[4],
                    Is.EqualTo(5)
                );
            }

            using (var valueList = new UnmanagedValueList<int>())
            {
                valueList.Add(6);

                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)1)
                       .And.Count.EqualTo((nuint)1)
                );

                Assert.That(() => valueList[0],
                    Is.EqualTo(6)
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.Clear" /> method.</summary>
        [Test]
        public static void ClearTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                valueList.Clear();

                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueList = new UnmanagedValueList<int>())
            {
                valueList.Clear();

                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.Contains(T)" /> method.</summary>
        [Test]
        public static void ContainsTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                Assert.That(() => valueList.Contains(1),
                    Is.True
                );

                Assert.That(() => valueList.Contains(4),
                    Is.False
                );
            }

            using (var valueList = new UnmanagedValueList<int>())
            {
                Assert.That(() => valueList.Contains(0),
                    Is.False
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.CopyTo(UnmanagedSpan{T})" /> method.</summary>
        [Test]
        public static void CopyToUnmanagedSpanTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                using (var destination = new UnmanagedArray<int>(3))
                {
                    valueList.CopyTo(destination);

                    Assert.That(() => destination[0],
                        Is.EqualTo(1)
                    );

                    Assert.That(() => destination[1],
                        Is.EqualTo(2)
                    );

                    Assert.That(() => destination[2],
                        Is.EqualTo(3)
                    );
                }

                using (var destination = new UnmanagedArray<int>(6))
                {
                    valueList.CopyTo(destination);

                    Assert.That(() => destination[0],
                        Is.EqualTo(1)
                    );

                    Assert.That(() => destination[1],
                        Is.EqualTo(2)
                    );

                    Assert.That(() => destination[2],
                        Is.EqualTo(3)
                    );

                    Assert.That(() => destination[3],
                        Is.EqualTo(0)
                    );

                    Assert.That(() => destination[4],
                        Is.EqualTo(0)
                    );

                    Assert.That(() => destination[5],
                        Is.EqualTo(0)
                    );
                }

                Assert.That(() => valueList.CopyTo(UnmanagedArray<int>.Empty),
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                          .And.Property("ActualValue").EqualTo((nuint)3)
                          .And.Property("ParamName").EqualTo("Count")
                );
            }

            using (var valueList = new UnmanagedValueList<int>())
            {
                Assert.That(() => valueList.CopyTo(UnmanagedArray<int>.Empty),
                    Throws.Nothing
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.EnsureCapacity(nuint)" /> method.</summary>
        [Test]
        public static void EnsureCapacityTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using var valueList = new UnmanagedValueList<int>(array);
            valueList.EnsureCapacity(0);

            Assert.That(() => valueList,
                Has.Property("Capacity").EqualTo((nuint)3)
                   .And.Count.EqualTo((nuint)3)
            );

            valueList.EnsureCapacity(3);

            Assert.That(() => valueList,
                Has.Property("Capacity").EqualTo((nuint)3)
                   .And.Count.EqualTo((nuint)3)
            );

            valueList.EnsureCapacity(4);

            Assert.That(() => valueList,
                Has.Property("Capacity").EqualTo((nuint)6)
                   .And.Count.EqualTo((nuint)3)
            );

            valueList.EnsureCapacity(16);

            Assert.That(() => valueList,
                Has.Property("Capacity").EqualTo((nuint)16)
                   .And.Count.EqualTo((nuint)3)
            );
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.Insert(nuint, T)" /> method.</summary>
        [Test]
        public static void InsertTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                valueList.Insert(3, 4);

                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)6)
                       .And.Count.EqualTo((nuint)4)
                );

                Assert.That(() => valueList[3],
                    Is.EqualTo(4)
                );

                Assert.That(() => valueList.Insert(5, 5),
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                          .And.Property("ActualValue").EqualTo((nuint)5)
                          .And.Property("ParamName").EqualTo("index")
                );
            }

            using (var valueList = new UnmanagedValueList<int>())
            {
                valueList.Insert(0, 1);

                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)1)
                       .And.Count.EqualTo((nuint)1)
                );

                Assert.That(() => valueList[0],
                    Is.EqualTo(1)
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.Remove(T)" /> method.</summary>
        [Test]
        public static void RemoveTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                Assert.That(() => valueList.Remove(1),
                    Is.True
                );

                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)2)
                );

                Assert.That(() => valueList[0],
                    Is.EqualTo(2)
                );

                Assert.That(() => valueList[1],
                    Is.EqualTo(3)
                );

                Assert.That(() => valueList.Remove(0),
                    Is.False
                );
            }

            using (var valueList = new UnmanagedValueList<int>())
            {
                Assert.That(() => valueList.Remove(0),
                    Is.False
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.RemoveAt(nuint)" /> method.</summary>
        [Test]
        public static void RemoveAtTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                valueList.RemoveAt(0);

                Assert.That(() => valueList,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)2)
                );

                Assert.That(() => valueList[0],
                    Is.EqualTo(2)
                );

                Assert.That(() => valueList[1],
                    Is.EqualTo(3)
                );

                Assert.That(() => valueList.RemoveAt(2),
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                          .And.Property("ActualValue").EqualTo((nuint)2)
                          .And.Property("ParamName").EqualTo("index")
                );
            }

            using (var valueList = new UnmanagedValueList<int>())
            {
                Assert.That(() => valueList.RemoveAt(0),
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                          .And.Property("ActualValue").EqualTo((nuint)0)
                          .And.Property("ParamName").EqualTo("index")
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueList{T}.TryGetIndexOf(T, out nuint)" /> method.</summary>
        [Test]
        public static void TryGetIndexOfTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueList = new UnmanagedValueList<int>(array, takeOwnership: true))
            {
                var result = valueList.TryGetIndexOf(1, out var index);

                Assert.That(() => result,
                    Is.True
                );

                Assert.That(() => index,
                    Is.EqualTo((nuint)0)
                );

                Assert.That(() => valueList.TryGetIndexOf(4, out _),
                    Is.False
                );
            }

            using (var valueList = new UnmanagedValueList<int>())
            {
                Assert.That(() => valueList.TryGetIndexOf(0, out _),
                    Is.False
                );
            }
        }
    }
}
