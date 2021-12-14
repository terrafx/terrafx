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
            yield return new TestCaseData(Quaternion.Create(0, 0, 0, 1), Vector3.Create(0, 0, 0), Vector3.Create(1, 1, 1), Vector3.Create(0, 0, 0));
            yield return new TestCaseData(Quaternion.Create(1, 0, 0, 1), Vector3.Create(0, 0, 0), Vector3.Create(7, 8, 9), Vector3.Create(1, 2, 3));
        }
    }

    /// <summary>Ensures that a instance's components are equal to the parameters used to construct one.</summary>
    [Test, TestCaseSource(nameof(TransformConstructorData))]
    public static void ComponentsReturnCorrectValues(Quaternion rotation, Vector3 rotationOrigin, Vector3 scale, Vector3 translation)
    {
        var transform = AffineTransform.Create(rotation, scale, translation);

        Assert.That(transform.Rotation, Is.EqualTo(rotation));
        Assert.That(transform.Scale, Is.EqualTo(scale));
        Assert.That(transform.Translation, Is.EqualTo(translation));
    }

    /// <summary>Ensures that two instances that are expected to compare equal do, in fact compare equal.</summary>
    [Test, TestCaseSource(nameof(TransformConstructorData))]
    public static void TransformsCompareEqual(Quaternion rotation, Vector3 rotationOrigin, Vector3 scale, Vector3 translation)
    {
        var a = AffineTransform.Create(rotation, scale, translation);
        var b = AffineTransform.Create(rotation, scale, translation);
        var c = AffineTransform.Create(rotation.WithY(1), scale, translation);

        Assert.That(a, Is.EqualTo(b));
        Assert.That(a, Is.Not.EqualTo(c));
    }

    /// <summary>Ensures that two instances that are expected to compare not equal do, in fact compare not equal.</summary>
    [Test, TestCaseSource(nameof(TransformConstructorData))]
    public static void TransformsCompareNotEqual(Quaternion rotation, Vector3 rotationOrigin, Vector3 scale, Vector3 translation)
    {
        var a = AffineTransform.Create(rotation, scale, translation);
        var b = AffineTransform.Create(rotation, scale, translation);
        var c = AffineTransform.Create(rotation.WithY(1), scale, translation);

        Assert.That(a, Is.Not.EqualTo(c));
        Assert.That(a, Is.EqualTo(b));
    }
}
