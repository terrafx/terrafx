using System;
using System.Collections.Generic;
using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Unit tests for <see cref="AffineTransform" />.</summary>
public class AffineTransformTests
{
    private static IEnumerable<TestCaseData> TransformConstructorData
    {
        get
        {
            yield return new TestCaseData(new Quaternion(0, 0, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0));
            yield return new TestCaseData(new Quaternion(1, 0, 0, 1), new Vector3(0, 0, 0), new Vector3(7, 8, 9), new Vector3(1, 2, 3));
        }
    }

    /// <summary>Ensures that a instance's components are equal to the parameters used to construct one.</summary>
    [Test, TestCaseSource(nameof(TransformConstructorData))]
    public static void ComponentsReturnCorrectValues(Quaternion rotation, Vector3 rotationOrigin, Vector3 scale, Vector3 translation)
    {
        var transform = new AffineTransform(rotation, rotationOrigin, scale, translation);

        Assert.That(transform.Rotation, Is.EqualTo(rotation));
        Assert.That(transform.RotationOrigin, Is.EqualTo(rotationOrigin));
        Assert.That(transform.Scale, Is.EqualTo(scale));
        Assert.That(transform.Translation, Is.EqualTo(translation));
    }

    /// <summary>Ensures that two instances that are expected to compare equal do, in fact compare equal.</summary>
    [Test, TestCaseSource(nameof(TransformConstructorData))]
    public static void TransformsCompareEqual(Quaternion rotation, Vector3 rotationOrigin, Vector3 scale, Vector3 translation)
    {
        var a = new AffineTransform(rotation, rotationOrigin, scale, translation);
        var b = new AffineTransform(rotation, rotationOrigin, scale, translation);
        var c = new AffineTransform(rotation.WithY(1), rotationOrigin, scale, translation);

        Assert.That(a, Is.EqualTo(b));
        Assert.That(a, Is.Not.EqualTo(c));
    }

    /// <summary>Ensures that two instances that are expected to compare not equal do, in fact compare not equal.</summary>
    [Test, TestCaseSource(nameof(TransformConstructorData))]
    public static void TransformsCompareNotEqual(Quaternion rotation, Vector3 rotationOrigin, Vector3 scale, Vector3 translation)
    {
        var a = new AffineTransform(rotation, rotationOrigin, scale, translation);
        var b = new AffineTransform(rotation, rotationOrigin, scale, translation);
        var c = new AffineTransform(rotation.WithY(1), rotationOrigin, scale, translation);

        Assert.That(a, Is.Not.EqualTo(c));
        Assert.That(a, Is.EqualTo(b));
    }
}
