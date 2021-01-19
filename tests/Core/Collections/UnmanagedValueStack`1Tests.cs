// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections
{
    /// <summary>Provides a set of tests covering the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
    public static class UnmanagedValueStackTests
    {
        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.UnmanagedValueStack()" /> constructor.</summary>
        [Test]
        public static void CtorTest()
        {
            using var valueStack = new UnmanagedValueStack<int>();

            Assert.That(() => valueStack,
                Has.Property("Capacity").EqualTo((nuint)0)
                   .And.Count.EqualTo((nuint)0)
            );
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.UnmanagedValueStack(nuint, nuint)" /> constructor.</summary>
        [Test]
        public static void CtorNUIntNUIntTest()
        {
            using (var valueStack = new UnmanagedValueStack<int>(5))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)5)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(5, 2))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)5)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueStack<int>(5, 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );

            using (var valueStack = new UnmanagedValueStack<int>(0, 2))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueStack<int>(0, 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.UnmanagedValueStack(UnmanagedReadOnlySpan{T}, nuint)" /> constructor.</summary>
        [Test]
        public static void CtorUnmanagedReadOnlySpanNUIntTest()
        {
            using var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array.AsSpan()))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)3)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(array.AsSpan(), 2))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)3)
                );
            }

            Assert.That(() => new UnmanagedValueStack<int>(array.AsSpan(), 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );

            using (var valueStack = new UnmanagedValueStack<int>(UnmanagedArray<int>.Empty.AsSpan()))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(UnmanagedArray<int>.Empty.AsSpan(), 2))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueStack<int>(UnmanagedArray<int>.Empty.AsSpan(), 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );

            using (var valueStack = new UnmanagedValueStack<int>(new UnmanagedArray<int>().AsSpan()))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(new UnmanagedArray<int>().AsSpan(), 2))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueStack<int>(new UnmanagedArray<int>().AsSpan(), 3),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .And.Property("ActualValue").EqualTo((nuint)3)
                        .And.Property("ParamName").EqualTo("alignment")
            );
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.UnmanagedValueStack(UnmanagedArray{T}, bool)" /> constructor.</summary>
        [Test]
        public static void CtorUnmanagedArrayBooleanTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: false))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)3)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: true))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)3)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(UnmanagedArray<int>.Empty, takeOwnership: false))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(UnmanagedArray<int>.Empty, takeOwnership: true))
            {
                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            Assert.That(() => new UnmanagedValueStack<int>(new UnmanagedArray<int>(), takeOwnership: false),
                Throws.ArgumentNullException
                      .And.Property("ParamName").EqualTo("array")
            );

            Assert.That(() => new UnmanagedValueStack<int>(new UnmanagedArray<int>(), takeOwnership: true),
                Throws.ArgumentNullException
                        .And.Property("ParamName").EqualTo("array")
            );
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.Clear" /> method.</summary>
        [Test]
        public static void ClearTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: false))
            {
                valueStack.Clear();

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: false))
            {
                _ = valueStack.Pop();

                valueStack.Push(4);
                valueStack.Clear();

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: true))
            {
                _ = valueStack.Pop();

                valueStack.Push(4);
                valueStack.Push(5);

                valueStack.Clear();

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)6)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>())
            {
                valueStack.Clear();

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)0)
                       .And.Count.EqualTo((nuint)0)
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.Contains(T)" /> method.</summary>
        [Test]
        public static void ContainsTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: true))
            {
                Assert.That(() => valueStack.Contains(1),
                    Is.True
                );

                Assert.That(() => valueStack.Contains(4),
                    Is.False
                );

                _ = valueStack.Pop();
                valueStack.Push(4);

                Assert.That(() => valueStack.Contains(3),
                    Is.False
                );

                Assert.That(() => valueStack.Contains(4),
                    Is.True
                );

                _ = valueStack.Pop();
                valueStack.Push(5);
                valueStack.Push(6);

                Assert.That(() => valueStack.Contains(4),
                    Is.False
                );

                Assert.That(() => valueStack.Contains(5),
                    Is.True
                );

                Assert.That(() => valueStack.Contains(6),
                    Is.True
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>())
            {
                Assert.That(() => valueStack.Contains(0),
                    Is.False
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.CopyTo(UnmanagedSpan{T})" /> method.</summary>
        [Test]
        public static void CopyToUnmanagedSpanTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: true))
            {
                using (var destination = new UnmanagedArray<int>(3))
                {
                    valueStack.CopyTo(destination);

                    Assert.That(() => destination[0],
                        Is.EqualTo(1)
                    );

                    Assert.That(() => destination[1],
                        Is.EqualTo(2)
                    );

                    Assert.That(() => destination[2],
                        Is.EqualTo(3)
                    );

                    _ = valueStack.Pop();
                    valueStack.Push(4);

                    valueStack.CopyTo(destination);

                    Assert.That(() => destination[0],
                        Is.EqualTo(1)
                    );

                    Assert.That(() => destination[1],
                        Is.EqualTo(2)
                    );

                    Assert.That(() => destination[2],
                        Is.EqualTo(4)
                    );
                }

                using (var destination = new UnmanagedArray<int>(6))
                {
                    valueStack.CopyTo(destination);

                    Assert.That(() => destination[0],
                        Is.EqualTo(1)
                    );

                    Assert.That(() => destination[1],
                        Is.EqualTo(2)
                    );

                    Assert.That(() => destination[2],
                        Is.EqualTo(4)
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

                    _ = valueStack.Pop();
                    valueStack.Push(5);
                    valueStack.Push(6);

                    valueStack.CopyTo(destination);

                    Assert.That(() => destination[0],
                        Is.EqualTo(1)
                    );

                    Assert.That(() => destination[1],
                        Is.EqualTo(2)
                    );

                    Assert.That(() => destination[2],
                        Is.EqualTo(5)
                    );

                    Assert.That(() => destination[3],
                        Is.EqualTo(6)
                    );

                    Assert.That(() => destination[4],
                        Is.EqualTo(0)
                    );

                    Assert.That(() => destination[5],
                        Is.EqualTo(0)
                    );
                }

                Assert.That(() => valueStack.CopyTo(UnmanagedArray<int>.Empty),
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                          .And.Property("ActualValue").EqualTo((nuint)4)
                          .And.Property("ParamName").EqualTo("Count")
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>())
            {
                Assert.That(() => valueStack.CopyTo(UnmanagedArray<int>.Empty),
                    Throws.Nothing
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.EnsureCapacity(nuint)" /> method.</summary>
        [Test]
        public static void EnsureCapacityTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using var valueStack = new UnmanagedValueStack<int>(array);
            valueStack.EnsureCapacity(0);

            Assert.That(() => valueStack,
                Has.Property("Capacity").EqualTo((nuint)3)
                   .And.Count.EqualTo((nuint)3)
            );

            valueStack.EnsureCapacity(3);

            Assert.That(() => valueStack,
                Has.Property("Capacity").EqualTo((nuint)3)
                   .And.Count.EqualTo((nuint)3)
            );

            valueStack.EnsureCapacity(4);

            Assert.That(() => valueStack,
                Has.Property("Capacity").EqualTo((nuint)6)
                   .And.Count.EqualTo((nuint)3)
            );

            valueStack.EnsureCapacity(16);

            Assert.That(() => valueStack,
                Has.Property("Capacity").EqualTo((nuint)16)
                   .And.Count.EqualTo((nuint)3)
            );
        }

        /// <summary>Provides validation of the <see cref="ValueStack{T}.Peek()" /> method.</summary>
        [Test]
        public static void PeekTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: true))
            {
                Assert.That(() => valueStack.Peek(),
                    Is.EqualTo(3)
                );

                valueStack.Push(4);

                Assert.That(() => valueStack.Peek(),
                    Is.EqualTo(4)
                );

                _ = valueStack.Pop();
                valueStack.Push(5);

                Assert.That(() => valueStack.Peek(),
                    Is.EqualTo(5)
                );

                _ = valueStack.Pop();
                valueStack.Push(6);
                valueStack.Push(7);

                Assert.That(() => valueStack.Peek(),
                    Is.EqualTo(7)
                );

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)6)
                       .And.Count.EqualTo((nuint)5)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>())
            {
                Assert.That(() => valueStack.Peek(),
                    Throws.InvalidOperationException
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.Pop()" /> method.</summary>
        [Test]
        public static void PopTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: false))
            {
                Assert.That(() => valueStack.Pop(),
                    Is.EqualTo(3)
                );

                Assert.That(() => valueStack.Pop(),
                    Is.EqualTo(2)
                );

                Assert.That(() => valueStack.Pop(),
                    Is.EqualTo(1)
                );

                Assert.That(() => valueStack.Pop(),
                    Throws.InvalidOperationException
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: true))
            {
                _ = valueStack.Pop();
                valueStack.Push(4);

                Assert.That(() => valueStack.Pop(),
                    Is.EqualTo(4)
                );

                _ = valueStack.Pop();
                valueStack.Push(5);
                valueStack.Push(6);

                Assert.That(() => valueStack.Pop(),
                    Is.EqualTo(6)
                );

                Assert.That(() => valueStack.Pop(),
                    Is.EqualTo(5)
                );

                Assert.That(() => valueStack.Pop(),
                    Is.EqualTo(1)
                );

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)0)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>())
            {
                Assert.That(() => valueStack.Pop(),
                    Throws.InvalidOperationException
                );
            }
        }

        /// <summary>Provides validation of the <see cref="UnmanagedValueStack{T}.Push(T)" /> method.</summary>
        [Test]
        public static void PushTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: false))
            {
                valueStack.Push(4);

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)6)
                       .And.Count.EqualTo((nuint)4)
                );

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)6)
                       .And.Count.EqualTo((nuint)4)
                );

                valueStack.Push(5);

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)6)
                       .And.Count.EqualTo((nuint)5)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: true))
            {
                _ = valueStack.Pop();
                valueStack.Push(5);

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)3)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>())
            {
                valueStack.Push(6);

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)1)
                       .And.Count.EqualTo((nuint)1)
                );
            }
        }

        /// <summary>Provides validation of the <see cref="ValueStack{T}.TryPeek(out T)" /> method.</summary>
        [Test]
        public static void TryPeekTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: true))
            {
                var result = valueStack.TryPeek(out var value);

                Assert.That(() => result,
                    Is.True
                );

                Assert.That(() => value,
                    Is.EqualTo(3)
                );

                valueStack.Push(4);
                result = valueStack.TryPeek(out value);

                Assert.That(() => result,
                    Is.True
                );

                Assert.That(() => value,
                    Is.EqualTo(4)
                );

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)6)
                       .And.Count.EqualTo((nuint)4)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>())
            {
                Assert.That(() => valueStack.TryPeek(out _),
                    Is.False
                );
            }
        }

        /// <summary>Provides validation of the <see cref="ValueStack{T}.TryPop(out T)" /> method.</summary>
        [Test]
        public static void TryPopTest()
        {
            var array = new UnmanagedArray<int>(3);

            array[0] = 1;
            array[1] = 2;
            array[2] = 3;

            using (var valueStack = new UnmanagedValueStack<int>(array, takeOwnership: true))
            {
                var result = valueStack.TryPop(out var value);

                Assert.That(() => result,
                    Is.True
                );

                Assert.That(() => value,
                    Is.EqualTo(3)
                );

                valueStack.Push(4);
                result = valueStack.TryPop(out value);

                Assert.That(() => result,
                    Is.True
                );

                Assert.That(() => value,
                    Is.EqualTo(4)
                );

                Assert.That(() => valueStack,
                    Has.Property("Capacity").EqualTo((nuint)3)
                       .And.Count.EqualTo((nuint)2)
                );
            }

            using (var valueStack = new UnmanagedValueStack<int>())
            {
                Assert.That(() => valueStack.TryPop(out _),
                    Is.False
                );
            }
        }
    }
}
