using FluentAssertions;
using Xunit;

namespace Utility.Security.Test;
public class JwtAuthTest
{
    private const string key = "capoo";

    [Fact]
    public void GenerateToken()
    {
        var result = new JwtAuth().GenerateToken(key, "啊啊啊啊啊啊啊啊");
        result.Should().Be("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZGVudGlmaWNhdGlvbiI6IuWViuWViuWViuWViuWViuWViuWViuWViiJ9.Fq55p1aTuNsw5SnGaa6uY2MAFK8xMigWREWbS7fTW8Y6T7LEG997c2fiQTKbILj2QTsc9l_P3ttXLuHpYC0NeA");
    }

    [Fact]
    public void AuthorizationToken()
    {
        var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZGVudGlmaWNhdGlvbiI6Iueyiee0heefruWGrOeTnCJ9.jR-qmPS9lJ_QgkcCtFZHyX8HvNiVscXLhojqKCRAKjTBr5Gq3N4-fmYm_IugYdC_zeKE1TCzu0VQIOnznW1nGQ";
        var result = new JwtAuth().AuthorizationToken(key, token);

        result.Should().BeTrue();
    }

    [Fact]
    public void TokenHasTimeout_Alive()
    {

        var token = new JwtAuth().GenerateToken(key, "我還沒過期", 100);
        var result = new JwtAuth().AuthorizationToken(key, token);

        result.Should().BeTrue();
    }

    [Fact]
    public void TokenHasTimeout_NotAlive()
    {

        var token = new JwtAuth().GenerateToken(key, "我過期了", -100);
        var result = new JwtAuth().AuthorizationToken(key, token);

        result.Should().BeFalse();
    }
}
